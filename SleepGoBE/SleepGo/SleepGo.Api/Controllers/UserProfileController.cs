using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.App.DTOs.ImageDtos;
using SleepGo.App.DTOs.UserProfileDtos;
using SleepGo.App.Features.UserProfiles.Commands;
using SleepGo.App.Features.UserProfiles.Queries;
using SleepGo.Domain.Entities;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/userProfiles")]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserProfile([FromForm] CreateUserProfileDto createUserProfileDto)
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file != null && file.Length > 0)
            {
                var imageDto = new CreateImageDto
                {
                    Name = file.FileName,
                    Type = file.ContentType,
                    Data = file
                };

                createUserProfileDto.ProfilePicture = imageDto;
            }

            var userProfile = await _mediator.Send(new CreateUserProfileCommand(createUserProfileDto));

            return Created(string.Empty, userProfile);
        }

        [HttpPut("{userProfileId}")]
        public async Task<IActionResult> UpdateUserProfile(Guid userProfileId, UpdateUserProfileDto updateUserProfileDto)
        {
            var updatedUser = await _mediator.Send(new UpdateUserProfileCommand(userProfileId, updateUserProfileDto));

            return Ok(updatedUser);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserProfileByUserId(Guid userId)
        {
            var userProfile = await _mediator.Send(new GetUserProfileByUserIdQuery(userId));

            return Ok(userProfile);
        }
    }
}
