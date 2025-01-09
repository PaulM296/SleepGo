using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.Api.Extensions;
using SleepGo.Api.Models;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Features.Users.Commands;
using SleepGo.App.Features.Users.Queries;

namespace SleepGo.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisteredUser([FromForm] RegisterUserDto registerUserDto)
        {
            var response = await _mediator.Send(new RegisterUserCommand(registerUserDto));

            var authenticationResult = new AuthenticationResult(response);

            return Ok(authenticationResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginDto loginDto)
        {
            var response = await _mediator.Send(new LoginUserCommand(loginDto));

            var authenticationResult = new AuthenticationResult(response);

            return Ok(authenticationResult);
        }

        [HttpPut("{userId}/block")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(Guid userId)
        {
            var response = await _mediator.Send(new BlockUserCommand(userId));

            return Ok(response);
        }

        [HttpPut("{userId}/unblock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnblockUser(Guid userId)
        {
            var response = await _mediator.Send(new UnblockUserCommand(userId));

            return Ok(response);
        }

        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto updateUserDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new UpdateUserCommand(userId, updateUserDto));

            return Ok(response);
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> RemoveUser()
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new RemoveUserCommand(userId));

            return Ok(response);
        }

        [HttpGet("logged-user")]
        [Authorize]
        public async Task<IActionResult> GetLoggedUserById()
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var users = await _mediator.Send(new GetUserByIdQuery(userId));

            return Ok(users);
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var users = await _mediator.Send(new GetUserByIdQuery(userId));

            return Ok(users);
        }

        [HttpGet("adminPage")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaginatedUsers([FromQuery] PaginationRequestDto paginationRequest)
        {
            var response = await _mediator.Send(new GetPaginatedUsersByIdQuery(paginationRequest));
            return Ok(response);
        }
    }
}
