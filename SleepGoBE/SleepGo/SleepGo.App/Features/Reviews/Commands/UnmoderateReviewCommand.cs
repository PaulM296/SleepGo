using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reviews.Commands
{
    public record UnmoderateReviewCommand(Guid reviewId) : IRequest<Unit>;

    public class UnmoderateReviewCommandHandler : IRequestHandler<UnmoderateReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnmoderateReviewCommandHandler> _logger;

        public UnmoderateReviewCommandHandler(IUnitOfWork unitOfWork, ILogger<UnmoderateReviewCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UnmoderateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(request.reviewId);

            if (review == null)
            {
                throw new ReviewNotFoundException($"Comment with ID {request.reviewId} has not been found!");
            }

            review.IsModerated = false;
            await _unitOfWork.ReviewRepository.UpdateAsync(review);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"Comment with ID {request.reviewId} has been unmoderated.");

            return Unit.Value;
        }
    }
}
