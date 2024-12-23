using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.DTOs.UserDtos;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Users.Commands
{
    public record LoginUserCommand(LoginDto loginUser) : IRequest<string>;

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IIdentityService _identityService;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(IAuthenticationService authenticationService, IIdentityService identityService,
            ILogger<LoginUserCommandHandler> logger)
        {
            _authenticationService = authenticationService;
            _identityService = identityService;
            _logger = logger;
        }
        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var createdUser = await _authenticationService.LoginUser(request.loginUser.Email, request.loginUser.Password);

            _logger.LogInformation("Login successful!");

            var claims = _identityService.CreateClaimsIdentity(createdUser);

            return _identityService.CreateSecurityToken(claims);
        }
    }
}
