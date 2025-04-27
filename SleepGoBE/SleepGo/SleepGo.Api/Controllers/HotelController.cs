using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.Api.Extensions;
using SleepGo.App.DTOs.HotelDtos;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.UserProfileDtos;
using SleepGo.App.Features.Hotels.Commands;
using SleepGo.App.Features.Hotels.Queries;
using SleepGo.App.Features.UserProfiles.Commands;
using SleepGo.App.Features.UserProfiles.Queries;
using SleepGo.App.Features.Users.Queries;

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

        [HttpPut("{hotelProfileId}")]
        public async Task<IActionResult> UpdateHotelProfile(Guid hotelProfileId, UpdateHotelDto updatehotelProfileDto)
        {
            var updatedHotel = await _mediator.Send(new UpdateHotelCommand(hotelProfileId, updatehotelProfileDto));

            return Ok(updatedHotel);
        }

        [HttpGet("{hotelId}")]
        public async Task<IActionResult> GetHotelProfileByUserId(Guid hotelId)
        {
            var hotelProfile = await _mediator.Send(new GetHotelProfileByIdQuery(hotelId));

            return Ok(hotelProfile);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllHotels([FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var hotels = await _mediator.Send(new GetPaginatedHotelsByIdQuery(paginationRequestDto));

            return Ok(hotels);
        }

        [HttpPost("hotel-recommendations/ask")]
        public async Task<IActionResult> AskQuestionsAboutHotel([FromBody] string question)
        {
            var result = await _mediator.Send(new AskQuestionsAboutHotelQuery(question));

            return Ok(result);
        }
    }
}
