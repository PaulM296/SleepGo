using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.App.DTOs.AmenityDtos;
using SleepGo.App.Features.Amenities.Commands;
using SleepGo.App.Features.Amenities.Queries;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/amenities")]
    [Authorize]
    public class AmenityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AmenityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAmenities([FromForm] CreateAmenityDto createAmenityDto)
        {
            var response = await _mediator.Send(new CreateAmenityCommand(createAmenityDto));

            return Ok(response);
        }

        [HttpPut("{amenityId}")]
        public async Task<IActionResult> UpdateAmenities(Guid amenityId, [FromBody] UpdateAmenityDto updateAmenityDto)
        {
            var response = await _mediator.Send(new UpdateAmenityCommand(amenityId, updateAmenityDto));

            return Ok(response);
        }

        [HttpDelete("{amenityId}")]
        public async Task<IActionResult> RemoveAmenity(Guid amenityId)
        {
            await _mediator.Send(new RemoveAmenityCommand(amenityId));

            return NoContent();
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetAmenitiesByHotelId(Guid hotelId)
        {
            var response = await _mediator.Send(new GetAmenitiesByHotelIdQuery(hotelId));

            return Ok(response);
        }
    }
}
