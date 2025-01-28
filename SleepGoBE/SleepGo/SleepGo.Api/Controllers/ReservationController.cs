using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.Api.Extensions;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.ReservationDtos;
using SleepGo.App.Features.Reservations.Commands;
using SleepGo.App.Features.Reservations.Queries;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/reservations")]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReservationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromForm] CreateReservationDto createReservationDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new CreateReservationCommand(userId, createReservationDto));

            return Ok(response);
        }

        [HttpPut("{reservationId}")]
        public async Task<IActionResult> UpdateReservation(Guid reservationId, [FromBody] UpdateReservationDto updateReservationDto)
        {
            var response = await _mediator.Send(new UpdateReservationCommand(reservationId, updateReservationDto));

            return Ok(response);
        }

        [HttpDelete("{reservationId}")]
        public async Task<IActionResult> RemoveReservation(Guid reservationId)
        {
            await _mediator.Send(new RemoveReservationCommand(reservationId));

            return NoContent();
        }

        [HttpGet("hotel/{hotelId}/")]
        public async Task<IActionResult> GetAllHotelReservations(Guid hotelId, [FromQuery] PaginationRequestDto paginationRequestDto) 
        {
            var response = await _mediator.Send(new GetAllHotelReservationsQuery(hotelId, paginationRequestDto));

            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAllUserReservations([FromQuery] PaginationRequestDto paginationRequestDto) 
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new GetAllUserReservationsQuery(userId, paginationRequestDto));

            return Ok(response);
        }
    }
}
