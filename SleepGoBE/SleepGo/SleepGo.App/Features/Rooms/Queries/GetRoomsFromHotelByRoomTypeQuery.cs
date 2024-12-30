using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.App.Exceptions;
using SleepGo.App.Features.Rooms.Commands;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.Domain.Enums;

namespace SleepGo.App.Features.Rooms.Queries
{
    public record GetRoomsFromHotelByRoomTypeQuery(Guid hotelId, RoomType roomType,
        PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseRoomDto>>;

    public class GetRoomsFromHotelByRoomTypeQueryHandler : IRequestHandler<GetRoomsFromHotelByRoomTypeQuery, PaginationResponseDto<ResponseRoomDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetRoomsFromHotelByRoomTypeQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetRoomsFromHotelByRoomTypeQueryHandler(IUnitOfWork unitOfWork, ILogger<GetRoomsFromHotelByRoomTypeQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseRoomDto>> Handle(GetRoomsFromHotelByRoomTypeQuery request, CancellationToken cancellationToken)
        {
            var rooms = await _unitOfWork.RoomRepository
                .GetAllPagedRoomsFromHotelByRoomTypeAsync(request.hotelId, request.roomType, 
                request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if (rooms.Items.Count == 0)
            {
                throw new RoomNotFoundException($"Could not retireve any rooms from hotel {request.hotelId} " +
                    $"with roomType {request.roomType}, because it doesn't have any yet!");
            }

            var roomDtos = new PaginationResponseDto<ResponseRoomDto>(
                items: _mapper.Map<List<ResponseRoomDto>>(rooms.Items),
                pageIndex: rooms.PageIndex,
                totalPages: rooms.TotalPages
                );

            _logger.LogInformation("All rooms have been successfully retrieved!");

            return roomDtos;
        }
    }
}
