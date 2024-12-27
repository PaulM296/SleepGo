using MediatR;
using Microsoft.Extensions.Logging;
using SleepGo.App.Exceptions;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reviews.Commands
{
    public record ModerateReviewCommand(Guid reviewId) : IRequest<Unit>;

    public class ModerateReviewCommandHandler : IRequestHandler<ModerateReviewCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ModerateReviewCommandHandler> _logger;

        public ModerateReviewCommandHandler(IUnitOfWork unitOfWork, ILogger<ModerateReviewCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(ModerateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.ReviewRepository.GetByIdAsync(request.reviewId);

            if(review == null)
            {
                throw new ReviewNotFoundException($"The review with ID {request.reviewId} has not been found!");
            }

            review.IsModerated = true;

            await _unitOfWork.ReviewRepository.UpdateAsync(review);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"The review with ID {request.reviewId} has been moderated!");

            return Unit.Value;
        }
    }
}
