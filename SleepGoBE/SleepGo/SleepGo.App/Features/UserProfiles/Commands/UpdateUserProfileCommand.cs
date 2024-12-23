using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserProfileDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.UserProfiles.Commands
{
    public record UpdateUserProfileCommand(Guid id, UpdateUserProfileDto updateUserProfileDto) : IRequest<ResponseUserProfileDto>;

    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ResponseUserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserProfileCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var getUserProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(request.id);

            if(getUserProfile == null)
            {
                throw new UserProfileNotFoundException($"The User with UserProfile Id {request.id} doesn't exist and it could not be updated!");
            }

            getUserProfile.ProfilePicture = request.updateUserProfileDto.ProfilePicture;
            getUserProfile.DateOfBirth = request.updateUserProfileDto.DateOfBirth;
            getUserProfile.FirstName = request.updateUserProfileDto.FirstName;
            getUserProfile.LastName = request.updateUserProfileDto.LastName;

            var updatedUserProfile = await _unitOfWork.UserProfileRepository.UpdateAsync(getUserProfile);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("UserProfile has been successfully updated!");

            return _mapper.Map<ResponseUserProfileDto>(updatedUserProfile);
        }
    }
}
