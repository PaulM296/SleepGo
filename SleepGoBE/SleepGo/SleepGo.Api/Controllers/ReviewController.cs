using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.Api.Extensions;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.ReviewDtos;
using SleepGo.App.Features.Reviews.Commands;
using SleepGo.App.Features.Reviews.Queries;
using SleepGo.App.Features.Users.Commands;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/reviews")]
    [Authorize]
    public class ReviewController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromForm] CreateReviewDto createReviewDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new CreateReviewCommand(userId, createReviewDto));

            return Ok(response);
        }

        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(Guid reviewId, [FromBody] UpdateReviewDto updateReviewDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new UpdateReviewCommand(userId, reviewId, updateReviewDto));

            return Ok(response);
        }

        [HttpPut("{reviewId}/moderate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ModerateReview(Guid reviewId)
        {
            var response = await _mediator.Send(new ModerateReviewCommand(reviewId));

            return Ok(response);
        }

        [HttpPut("{reviewId}/unmoderate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnmoderateReview(Guid reviewId)
        {
            var response = await _mediator.Send(new UnmoderateReviewCommand(reviewId));

            return Ok(response);
        }

        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> RemoveReview(Guid reviewId)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            await _mediator.Send(new RemoveReviewCommand(userId, reviewId));

            return NoContent();
        }

        [HttpGet("hotel/{hotelId}/reviews")]
        public async Task<IActionResult> GetAllHotelsReviews(Guid hotelId, [FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var response = await _mediator.Send(new GetAllHotelReviewsQuery(hotelId, paginationRequestDto));

            return Ok(response);
        }

        [HttpGet("user/{userId}/reviews")]
        public async Task<IActionResult> GetAllUserReviews(Guid userId, [FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var response = await _mediator.Send(new GetAllUserReviewsQuery(userId, paginationRequestDto));

            return Ok(response);
        }
    }
}
