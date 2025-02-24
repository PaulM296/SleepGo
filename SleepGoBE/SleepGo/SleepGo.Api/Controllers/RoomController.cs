using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.Api.Extensions;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.RoomDtos;
using SleepGo.App.Features.Rooms.Commands;
using SleepGo.App.Features.Rooms.Queries;
using SleepGo.Domain.Enums;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/rooms")]
    [Authorize]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromForm] CreateRoomDto createRoomDto)
        {
            var response = await _mediator.Send(new CreateRoomCommand(createRoomDto));

            return Ok(response);
        }

        [HttpPut("{roomId}")]
        public async Task<IActionResult> UpdateRoom(Guid roomId, [FromBody] UpdateRoomDto updateRoomDto)
        {
            var response = await _mediator.Send(new UpdateRoomCommand(roomId, updateRoomDto));

            return Ok(response);
        }

        [HttpDelete("{roomId}")]
        public async Task<IActionResult> RemoveRoom(Guid roomId)
        {
            await _mediator.Send(new RemoveRoomCommand(roomId));

            return NoContent();
        }

        [HttpGet("hotel/{hotelId}/available")] 
        public async Task<IActionResult> GetAvailableRoomsFromHotelByRoomType(Guid hotelId, [FromQuery] RoomType roomType,
            [FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var response = await _mediator.Send(new GetAvailableRoomsFromHotelByRoomTypeQuery(hotelId, roomType, paginationRequestDto));

            return Ok(response);
        }

        [HttpGet("hotel/{hotelId}/rooms")]
        public async Task<IActionResult> GetRoomsFromHotelByRoomType(Guid hotelId, [FromQuery] RoomType roomType, 
            [FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var response = await _mediator.Send(new GetRoomsFromHotelByRoomTypeQuery(hotelId, roomType, paginationRequestDto));

            return Ok(response);
        }

        [HttpGet("hotel/available/{hotelId}/rooms")]
        public async Task<IActionResult> GetAllAvailableRoomsFromHotelByHotelId(Guid hotelId)
        {
            var response = await _mediator.Send(new GetAllAvailableRoomsFromHotelByHotelIdQuery(hotelId));

            return Ok(response);
        }
    }
}
