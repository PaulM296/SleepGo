using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SleepGo.Api.Features.Queries;
using SleepGo.Api.Interfaces;
using SleepGo.Api.Services;
using SleepGo.MLTrainer.Models;

namespace SleepGo.Api.Controllers
{
    [Route("api/[controller]")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;
        private readonly IMediator _mediator;

        public RecommendationController(IRecommendationService recommendationService, IMediator mediator)
        {
            _recommendationService = recommendationService;
            _mediator = mediator;
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromBody] HotelRecommendationData input)
        {
            var result = _recommendationService.Predict(input);
            return Ok(result);
        }

        [HttpGet("recommend/{userId}")]
        public async Task<IActionResult> Recommend(Guid userId)
        {
            var result = await _mediator.Send(new GetUserRecommendationsQuery(userId));
            return Ok(result);
        }
    }
}
