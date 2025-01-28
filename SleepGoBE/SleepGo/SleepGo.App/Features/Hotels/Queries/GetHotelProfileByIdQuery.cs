using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.DTOs.UserProfileDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Features.UserProfiles.Queries;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Hotels.Queries
{
    public record GetHotelProfileByIdQuery(Guid hotelId) : IRequest<ResponseHotelDto>;

    public class GetHotelProfileByIdQueryHandler : IRequestHandler<GetHotelProfileByIdQuery, ResponseHotelDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetHotelProfileByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetHotelProfileByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetHotelProfileByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseHotelDto> Handle(GetHotelProfileByIdQuery request, CancellationToken cancellationToken)
        {
            var hotelProfile = await _unitOfWork.HotelRepository.GetHotelProfileByUserId(request.hotelId);

            if (hotelProfile == null)
            {
                throw new UserProfileNotFoundException($"The hotel profile with userId: {request.hotelId} has not been found!");
            }

            _logger.LogInformation("UserProfile successfully retrieved!");

            return _mapper.Map<ResponseHotelDto>(hotelProfile);
        }
    }
}
