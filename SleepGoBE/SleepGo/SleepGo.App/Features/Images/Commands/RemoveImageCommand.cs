using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Images.Commands
{
    public record RemoveImageCommand(Guid id) : IRequest<Unit>;

    public class RemoveImageCommandHandler : IRequestHandler<RemoveImageCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveImageCommandHandler> _logger;

        public RemoveImageCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveImageCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(RemoveImageCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ImageRepository.RemoveImage(request.id);

            return Unit.Value;
        }
    }
}
