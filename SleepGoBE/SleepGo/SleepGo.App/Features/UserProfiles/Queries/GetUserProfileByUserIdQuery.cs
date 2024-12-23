using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserProfileDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.UserProfiles.Queries
{
    public record GetUserProfileByUserIdQuery(Guid userId) : IRequest<ResponseUserProfileDto>;
    
    public class GetUserProfileByUserIdQueryHandler : IRequestHandler<GetUserProfileByUserIdQuery, ResponseUserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserProfileByUserIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserProfileByUserIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserProfileByUserIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseUserProfileDto> Handle(GetUserProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userProfile = await _unitOfWork.UserProfileRepository.GetUserProfileByUserId(request.userId);

            if (userProfile == null)
            {
                throw new UserProfileNotFoundException($"The user profile with userId: {request.userId} has not been found!");
            }

            _logger.LogInformation("UserProfile successfully retrieved!");

            return _mapper.Map<ResponseUserProfileDto>(userProfile);
        }
    }
}
