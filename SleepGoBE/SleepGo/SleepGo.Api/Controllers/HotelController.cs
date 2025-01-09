using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.Api.Extensions;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.Features.Hotels.Commands;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/hotels")]
    [Authorize]
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel([FromForm] CreateHotelDto createHotelDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new CreateHotelCommand(createHotelDto));

            return Ok(response);
        }
    }
}
