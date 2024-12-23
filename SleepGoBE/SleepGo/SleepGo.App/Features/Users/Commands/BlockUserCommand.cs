using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Users.Commands
{
    public record BlockUserCommand(Guid userId) : IRequest<Unit>;

    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BlockUserCommandHandler> _logger;

        public BlockUserCommandHandler(IUnitOfWork unitOfWork, ILogger<BlockUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if(user == null)
            {
                throw new UserNotFoundException($"User with ID {request.userId} has not been found!");
            }

            user.IsBlocked = true;
            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"User with ID {request.userId} has been blocked!");

            return Unit.Value;
        }
    }
}
