using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserProfileDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.UserProfiles.Commands
{
    public record CreateUserProfileCommand(CreateUserProfileDto createUserProfileDto) : IRequest<ResponseUserProfileDto>;
    
    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, ResponseUserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateUserProfileCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateUserProfileCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateUserProfileCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfileDto> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _unitOfWork.UserRepository.GetByIdAsync(request.createUserProfileDto.UserId);

            if (userExists == null)
            {
                throw new UserNotFoundException($"The user with ID {request.createUserProfileDto.UserId} doesn't exist!");
            }

            Image image = null;
            if (request.createUserProfileDto.ProfilePicture != null)
            {
                if (!IsValidImageFormat(request.createUserProfileDto.ProfilePicture.Type))
                {
                    throw new InvalidImageFormatException("Unsupported image format. Only PNG, JPEG, and JPG are supported.");
                }

                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    await request.createUserProfileDto.ProfilePicture.Data.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                image = new Image
                {
                    Name = request.createUserProfileDto.ProfilePicture.Name,
                    Type = request.createUserProfileDto.ProfilePicture.Type,
                    Data = imageData
                };

                await _unitOfWork.ImageRepository.UploadImage(image);
            }

            var userProfile = new UserProfile
            {
                UserId = request.createUserProfileDto.UserId,
                DateOfBirth = request.createUserProfileDto.DateOfBirth,
                FirstName = request.createUserProfileDto.FirstName,
                LastName = request.createUserProfileDto.LastName,
                ImageId = image?.Id ?? Guid.Empty,
                Image = image
            };

            try
            {
                await _unitOfWork.UserProfileRepository.AddAsync(userProfile);
                await _unitOfWork.SaveAsync();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred while saving the UserProfile entity.");
                throw;
            }

            _logger.LogInformation("New UserProfile has been successfully created!");

            return _mapper.Map<ResponseUserProfileDto>(userProfile);
        }

        private bool IsValidImageFormat(string imageType)
        {
            var supportedFormats = new[] { "image/png", "image/jpeg", "image/jpg" };
            return supportedFormats.Contains(imageType.ToLower());
        }
    }
}
