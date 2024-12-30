using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Enums;

namespace SleepGo.App.Features.Rooms.Queries
{
    public record GetAvailableRoomsFromHotelByRoomTypeQuery(Guid hotelId, RoomType roomType, 
        PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseRoomDto>>;

    public class GetAvailableRoomsFromHotelByRoomTypeQueryHandler : IRequestHandler<GetAvailableRoomsFromHotelByRoomTypeQuery, PaginationResponseDto<ResponseRoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAvailableRoomsFromHotelByRoomTypeQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAvailableRoomsFromHotelByRoomTypeQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAvailableRoomsFromHotelByRoomTypeQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseRoomDto>> Handle(GetAvailableRoomsFromHotelByRoomTypeQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _unitOfWork.RoomRepository
                .GetAllPagedAvailableRoomsFromHotelByRoomTypeAsync(request.hotelId, request.roomType,
                request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if(rooms.Items.Count == 0)
            {
                throw new NoAvailableRoomsException($"There aren't any available rooms of roomType {request.roomType} for hotel {request.hotelId}!");
            }

            var roomDtos = new PaginationResponseDto<ResponseRoomDto>(
                items: _mapper.Map<List<ResponseRoomDto>>(rooms.Items),
                pageIndex: rooms.PageIndex,
                totalPages: rooms.TotalPages
                );

            _logger.LogInformation("All available rooms have been successfully retrieved!");

            return roomDtos;
        }
    }
}
