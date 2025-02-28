using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Rooms.Queries
{
    public record GetAllRoomsFromHotelByHotelIdQuery(Guid hotelId) : IRequest<ICollection<ResponseRoomDto>>;

    public class GetAllRoomsFromHotelByHotelIdQueryHandler : IRequestHandler<GetAllRoomsFromHotelByHotelIdQuery, ICollection<ResponseRoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllRoomsFromHotelByHotelIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllRoomsFromHotelByHotelIdQueryHandler(IUnitOfWork unitOfWork,
            ILogger<GetAllRoomsFromHotelByHotelIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ICollection<ResponseRoomDto>> Handle(GetAllRoomsFromHotelByHotelIdQuery request, CancellationToken cancellationToken)
        {
            var hotelRooms = await _unitOfWork.RoomRepository.GetAllRoomsByHotelIdAsync(request.hotelId);

            if (hotelRooms == null)
            {
                throw new RoomNotFoundException("There are no available rooms at this time!");
            }

            _logger.LogInformation($"Hotel rooms successfully retrieved!");

            return _mapper.Map<ICollection<ResponseRoomDto>>(hotelRooms);
        }
    }
}
