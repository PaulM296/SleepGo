using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.Domain.Enums;

namespace SleepGo.App.Features.Users.Commands
{
    public record UpdateUserCommand(Guid userId, UpdateUserDto updateUserDto) : IRequest<object>;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, object>
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

        public async Task<object> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if (user == null)
            {
                throw new UserNotFoundException($"The user with ID {request.userId} doesn't exist and it could not be updated!");
            }

            Image image = null;
            if (request.updateUserDto.ProfilePicture != null)
            {
                var allowedFormats = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(request.updateUserDto.ProfilePicture.FileName).ToLower();
                if (!allowedFormats.Contains(fileExtension))
                {
                    throw new InvalidImageFormatException("Invalid image format. Only .png, .jpg, and .jpeg are allowed.");
                }

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

            if(user.Role == Role.User)
            {
                user.UserName = request.updateUserDto.UserName;
                user.NormalizedUserName = request.updateUserDto.UserName.ToUpper();
                user.Email = request.updateUserDto.Email;
                user.NormalizedEmail = request.updateUserDto.Email.ToUpper();
                user.PhoneNumber = request.updateUserDto.PhoneNumber;
                
                if(user.UserProfile != null)
                {
                    user.UserProfile.FirstName = request.updateUserDto.FirstName ?? user.UserProfile.FirstName;
                    user.UserProfile.LastName = request.updateUserDto.LastName ?? user.UserProfile.LastName;
                    user.UserProfile.DateOfBirth = request.updateUserDto.DateOfBirth ?? user.UserProfile.DateOfBirth;
                    user.UserProfile.ImageId = image?.Id ?? user.UserProfile.ImageId;
                }
            }
            else if(user.Role == Role.Hotel)
            {
                user.UserName = request.updateUserDto.UserName;
                user.NormalizedUserName = request.updateUserDto.UserName.ToUpper();
                user.Email = request.updateUserDto.Email;
                user.NormalizedEmail = request.updateUserDto.Email.ToUpper();
                user.PhoneNumber = request.updateUserDto.PhoneNumber;

                user.Hotel.HotelName = request.updateUserDto.HotelName ?? user.Hotel.HotelName;
                user.Hotel.HotelDescription = request.updateUserDto.HotelDescription ?? user.Hotel.HotelDescription;
                user.Hotel.City = request.updateUserDto.City ?? user.Hotel.City;
                user.Hotel.Country = request.updateUserDto.Country ?? user.Hotel.Country;
                user.Hotel.ZipCode = request.updateUserDto.ZipCode ?? user.Hotel.ZipCode;
                user.Hotel.Address = request.updateUserDto.Address ?? user.Hotel.Address;
                user.Hotel.Latitude = request.updateUserDto.Latitude ?? user.Hotel.Latitude;
                user.Hotel.Longitude = request.updateUserDto.Longitude ?? user.Hotel.Longitude;
                user.Hotel.ImageId = image?.Id ?? user.Hotel.ImageId;
            }

            var updatedUser = await _unitOfWork.UserRepository.UpdateUserAsync(user);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User has been successfully updated!");

            return user.Role == Role.Hotel
                ? _mapper.Map<ResponseHotelUserDto>(updatedUser)
                : _mapper.Map<ResponseUserDto>(updatedUser);
        }
    }
}
