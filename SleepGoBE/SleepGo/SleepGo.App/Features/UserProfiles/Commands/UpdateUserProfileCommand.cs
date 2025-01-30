using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserProfileDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using System.Linq;

namespace SleepGo.App.Features.UserProfiles.Commands
{
    public record UpdateUserProfileCommand(Guid id, UpdateUserProfileDto updateUserProfileDto) : IRequest<ResponseUserProfileDto>;

    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ResponseUserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserProfileCommandHandler> logger,
            IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ResponseUserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var getUserProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(request.id);

            if (getUserProfile == null)
            {
                throw new UserProfileNotFoundException($"The User with UserProfile Id {request.id} doesn't exist and it could not be updated!");
            }

            var user = await _userManager.FindByIdAsync(getUserProfile.UserId.ToString());
            if (user == null)
            {
                throw new UserProfileNotFoundException($"The associated AppUser for UserProfile Id {request.id} was not found.");
            }

            if (request.updateUserProfileDto.ProfilePictureFile != null)
            {
                var allowedFormats = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtension = Path.GetExtension(request.updateUserProfileDto.ProfilePictureFile.FileName).ToLower();
                if (!allowedFormats.Contains(fileExtension))
                {
                    throw new Exception("Invalid image format. Only .png, .jpg, and .jpeg are allowed.");
                }

                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    await request.updateUserProfileDto.ProfilePictureFile.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                var image = new Image
                {
                    Id = Guid.NewGuid(),
                    Name = request.updateUserProfileDto.ProfilePictureFile.FileName,
                    Type = request.updateUserProfileDto.ProfilePictureFile.ContentType,
                    Data = imageData
                };

                await _unitOfWork.ImageRepository.UploadImage(image);
                await _unitOfWork.SaveAsync();

                getUserProfile.ImageId = image.Id;
            }

            getUserProfile.ProfilePicture = request.updateUserProfileDto.ProfilePicture;
            getUserProfile.DateOfBirth = request.updateUserProfileDto.DateOfBirth;
            getUserProfile.FirstName = request.updateUserProfileDto.FirstName;
            getUserProfile.LastName = request.updateUserProfileDto.LastName;

            if (user.Email != request.updateUserProfileDto.Email)
            {
                var emailResult = await _userManager.SetEmailAsync(user, request.updateUserProfileDto.Email);
                if (!emailResult.Succeeded)
                {
                    var errors = string.Join(", ", emailResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update email: {errors}");
                }
            }

            if (user.PhoneNumber != request.updateUserProfileDto.PhoneNumber)
            {
                var phoneResult = await _userManager.SetPhoneNumberAsync(user, request.updateUserProfileDto.PhoneNumber);
                if (!phoneResult.Succeeded)
                {
                    var errors = string.Join(", ", phoneResult.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update phone number: {errors}");
                }
            }

            var updatedUserProfile = await _unitOfWork.UserProfileRepository.UpdateAsync(getUserProfile);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("UserProfile has been successfully updated!");

            return _mapper.Map<ResponseUserProfileDto>(updatedUserProfile);
        }
    }
}
