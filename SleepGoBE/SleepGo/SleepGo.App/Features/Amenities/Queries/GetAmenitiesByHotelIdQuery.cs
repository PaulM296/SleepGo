using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Amenities.Queries
{
    //public record GetAmenitiesByHotelIdQuery(Guid hotelId) : IRequest<ResponseAmenityDto>;
    public record GetAmenitiesByHotelIdQuery(Guid hotelId) : IRequest<List<ResponseAmenityDto>>;


    //public class GetAmenitiesByHotelIdQueryHandler : IRequestHandler<GetAmenitiesByHotelIdQuery, ResponseAmenityDto>
    public class GetAmenitiesByHotelIdQueryHandler : IRequestHandler<GetAmenitiesByHotelIdQuery, List<ResponseAmenityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAmenitiesByHotelIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAmenitiesByHotelIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAmenitiesByHotelIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        //public async Task<ResponseAmenityDto> Handle(GetAmenitiesByHotelIdQuery request, CancellationToken cancellationToken)
        public async Task<List<ResponseAmenityDto>> Handle(GetAmenitiesByHotelIdQuery request, CancellationToken cancellationToken)
        {
            var amenitiesForHotel = await _unitOfWork.AmenityRepository.GetHotelAmenitiesByHotelIdAsync(request.hotelId);

            if(amenitiesForHotel == null)
            {
                throw new AmenityNotFoundException($"The hotel with id {request.hotelId} does not have any amenities!");
            }

            _logger.LogInformation($"Hotel amenities successfully retrieved!");

            //return _mapper.Map<ResponseAmenityDto>(amenitiesForHotel);
            return _mapper.Map<List<ResponseAmenityDto>>(amenitiesForHotel);
        }
    }
}
