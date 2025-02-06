using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.App.DTOs.ReservationDtos;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Enums;

namespace SleepGo.App.Features.Users.Queries
{
    public record GetUserByIdQuery(Guid userId) : IRequest<object>;

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, object>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<object> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if(user == null)
            {
                throw new UserNotFoundException($"Could not get the user with Id {request.userId}, because it doesn't exist!");
            }

            _logger.LogInformation($"User successfully retrieved!");

            if (user.Role == Role.Hotel)
            {
                return _mapper.Map<ResponseHotelUserDto>(user);
            }

            return _mapper.Map<ResponseUserDto>(user);
        }
    }
}
