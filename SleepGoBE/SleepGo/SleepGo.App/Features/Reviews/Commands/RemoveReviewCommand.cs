using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reviews.Commands
{
    public record RemoveReviewCommand(Guid userId, Guid id) : IRequest<Unit>;

    public class RemoveReviewCommandHandler : IRequestHandler<RemoveReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveReviewCommandHandler> _logger;

        public RemoveReviewCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveReviewCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveReviewCommand request, CancellationToken cancellationToken)
        {
            var reviewToRemove = await _unitOfWork.ReviewRepository.GetByIdAsync(request.id);

            if(reviewToRemove == null)
            {
                throw new ReviewNotFoundException($"The review with ID {request.id} has not been found and therefore could not be removed!");
            }

            await _unitOfWork.ReviewRepository.RemoveAsync(reviewToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Review removed successfully!");

            return Unit.Value;
        }
    }
}
