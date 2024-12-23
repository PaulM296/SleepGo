using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;

namespace SleepGo.App.Features.Users.Commands
{
    public record RegisterUserCommand(RegisterUserDto registerUser) : IRequest<string>;

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IIdentityService _identityService;

        public RegisterUserCommandHandler(IAuthenticationService authenticationService, IUnitOfWork unitOfWork,
            ILogger<RegisterUserCommandHandler> logger, IIdentityService identityService)
        {
            _authenticationService = authenticationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _identityService = identityService;
        }

        public async Task<string> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var newUser = new AppUser
            {
                UserName = request.registerUser.UserName,
                Email = request.registerUser.Email,
                Role = request.registerUser.Role
            };

            Image image = null;
            
            if(request.registerUser.ProfilePicture != null)
            {
                //Validate image format
                var allowedFormats = new[] { ".png", ".jpg", ".jpeg" };
                var fileExtenstion = Path.GetExtension(request.registerUser.ProfilePicture.FileName).ToLower();

                if(!allowedFormats.Contains(fileExtenstion))
                {
                    throw new InvalidImageFormatException("Invalid image format. Only .png, .jpg, and .jpeg are allowed.");
                }

                //Convert image to binary format
                byte[] imageData;
                using (var memoryStream = new MemoryStream())
                {
                    await request.registerUser.ProfilePicture.CopyToAsync(memoryStream);
                    imageData = memoryStream.ToArray();
                }

                image = new Image
                {
                    Name = request.registerUser.ProfilePicture.FileName,
                    Type = request.registerUser.ProfilePicture.ContentType,
                    Data = imageData
                };

                await _unitOfWork.ImageRepository.UploadImage(image);
                await _unitOfWork.SaveAsync();
            }

            var newUserProfile = new UserProfile
            {
                FirstName = request.registerUser.FirstName,
                LastName = request.registerUser.LastName,
                DateOfBirth = request.registerUser.DateOfBirth,
                ImageId = image?.Id ?? Guid.Empty
            };

            var createdUser = await _authenticationService.Register(newUser, newUserProfile, request.registerUser.Password);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("New user successfully added!");

            var claims = _identityService.CreateClaimsIdentity(createdUser);

            return _identityService.CreateSecurityToken(claims);
        }
    }
}
