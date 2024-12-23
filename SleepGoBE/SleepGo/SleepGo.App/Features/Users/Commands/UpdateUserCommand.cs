using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Users.Commands
{
    public record UpdateUserCommand(Guid userId, UpdateUserDto updateUserDto) : IRequest<ResponseUserDto>;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ResponseUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if (user == null)
            {
                throw new UserNotFoundException($"The user with ID {request.userId} doesn't exist and it could not be updated!");
            }

            Image image = null;
            if (request.updateUserDto.ProfilePicture != null)
            {
                // Validate image format
                var allowedFormats = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(request.updateUserDto.ProfilePicture.FileName).ToLower();
                if (!allowedFormats.Contains(fileExtension))
                {
                    throw new InvalidImageFormatException("Invalid image format. Only .png, .jpg, and .jpeg are allowed.");
                }

                // Convert image to binary format
                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    await request.updateUserDto.ProfilePicture.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                image = new Image
                {
                    Name = request.updateUserDto.ProfilePicture.FileName,
                    Type = request.updateUserDto.ProfilePicture.ContentType,
                    Data = imageData
                };

                await _unitOfWork.ImageRepository.UploadImage(image);
                await _unitOfWork.SaveAsync();
            }

            user.UserName = request.updateUserDto.UserName;
            user.NormalizedUserName = request.updateUserDto.UserName.ToUpper();
            user.Email = request.updateUserDto.Email;
            user.NormalizedEmail = request.updateUserDto.Email.ToUpper();
            user.Role = request.updateUserDto.Role;
            user.UserProfile.ImageId = image?.Id ?? Guid.Empty;
            user.UserProfile.FirstName = request.updateUserDto.FirstName;
            user.UserProfile.LastName = request.updateUserDto.LastName;

            var updatedUser = await _unitOfWork.UserRepository.UpdateUserAsync(user);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User has been successfully updated!");

            return _mapper.Map<ResponseUserDto>(updatedUser);
        }
    }
}
