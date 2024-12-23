using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Users.Commands
{
    public record UnblockUserCommand(Guid userId) : IRequest<Unit>;

    public class UnblockUserCommandHandler : IRequestHandler<UnblockUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnblockUserCommandHandler> _logger;

        public UnblockUserCommandHandler(IUnitOfWork unitOfWork, ILogger<UnblockUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if(user == null)
            {
                throw new UserNotFoundException($"User with ID {request.userId} has not been found!");
            }

            user.IsBlocked = false;
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"User with ID {request.userId} has been unblocked.");

            return Unit.Value;
        }
    }
}
