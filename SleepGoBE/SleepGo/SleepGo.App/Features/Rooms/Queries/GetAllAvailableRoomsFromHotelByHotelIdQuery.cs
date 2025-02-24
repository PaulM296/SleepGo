using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Rooms.Queries
{
    public record GetAllAvailableRoomsFromHotelByHotelIdQuery(Guid hotelId) : IRequest<ICollection<ResponseRoomDto>>;

    public class GetAllAvailableRoomsFromHotelByHotelIdQueryHandler : IRequestHandler<GetAllAvailableRoomsFromHotelByHotelIdQuery, ICollection<ResponseRoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllAvailableRoomsFromHotelByHotelIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllAvailableRoomsFromHotelByHotelIdQueryHandler(IUnitOfWork unitOfWork, 
            ILogger<GetAllAvailableRoomsFromHotelByHotelIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ICollection<ResponseRoomDto>> Handle(GetAllAvailableRoomsFromHotelByHotelIdQuery request, CancellationToken cancellationToken)
        {
            var hotelRooms = await _unitOfWork.RoomRepository.GetAvailableRoomsByHotelIdAsync(request.hotelId);

            if(hotelRooms == null)
            {
                throw new RoomNotFoundException("There are no available rooms at this time!");
            }

            _logger.LogInformation($"Hotel rooms successfully retrieved!");

            return _mapper.Map<ICollection<ResponseRoomDto>>(hotelRooms);
        }
    }
}
