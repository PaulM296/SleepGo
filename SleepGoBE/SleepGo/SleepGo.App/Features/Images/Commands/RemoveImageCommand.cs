using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
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
            var image = await _unitOfWork.ImageRepository.GetImageById(request.id);
            if(image == null)
            {
                throw new ImageNotFoundException($"Image with ID {request.id} has not been found!");
            }
            
            await _unitOfWork.ImageRepository.RemoveImage(request.id);

            return Unit.Value;
        }
    }
}
