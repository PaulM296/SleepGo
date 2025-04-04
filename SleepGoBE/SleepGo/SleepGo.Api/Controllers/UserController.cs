﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SleepGo.Api.Extensions;
using SleepGo.Api.Models;
using SleepGo.App.DTOs.PaginationDtos;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Features.Hotels.Queries;
using SleepGo.App.Features.Users.Commands;
using SleepGo.App.Features.Users.Queries;
using SleepGo.Domain.Enums;

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
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUserDto registerUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (registerUserDto.Role == Role.Hotel)
            {
                if (string.IsNullOrEmpty(registerUserDto.HotelName) ||
                    string.IsNullOrEmpty(registerUserDto.Address) ||
                    string.IsNullOrEmpty(registerUserDto.City) ||
                    string.IsNullOrEmpty(registerUserDto.Country) ||
                    string.IsNullOrEmpty(registerUserDto.ZipCode))
                {
                    return BadRequest("All hotel-related fields must be provided for a hotel user.");
                }
            }

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

        [HttpGet("adminPage/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPaginatedUsers([FromQuery] PaginationRequestDto paginationRequest)
        {
            var response = await _mediator.Send(new GetPaginatedUsersByIdQuery(paginationRequest));
            return Ok(response);
        }

        [HttpGet("adminPage/hotels")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllHotels([FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var hotels = await _mediator.Send(new GetPaginatedHotelsByIdQuery(paginationRequestDto));

            return Ok(hotels);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchHotels([FromQuery] string query, [FromQuery] PaginationRequestDto paginationRequestDto)
        {
            var hotelUsers = await _mediator.Send(new SearchHotelsQuery(query, paginationRequestDto));
            return Ok(hotelUsers);
        }
    }
}
