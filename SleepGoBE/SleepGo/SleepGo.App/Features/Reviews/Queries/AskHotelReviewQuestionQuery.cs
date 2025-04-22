using MediatR;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reviews.Queries
{
    public record AskHotelReviewQuestionQuery(Guid HotelId, string Question): IRequest<string>;

    public class AskHotelReviewQuestionQueryHandler : IRequestHandler<AskHotelReviewQuestionQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOpenAIService _openAIService;

        public AskHotelReviewQuestionQueryHandler(IUnitOfWork unitOfWork, IOpenAIService openAIService)
        {
            _unitOfWork = unitOfWork;
            _openAIService = openAIService;
        }

        public async Task<string> Handle(AskHotelReviewQuestionQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _unitOfWork.ReviewRepository.GetAllReviewsByHotelIdAsync(request.HotelId);

            if(!reviews.Any())
            {
                return "There are no reviews available for this hotel yet.";
            }

            var reviewTexts = reviews
                .Where(r => !string.IsNullOrWhiteSpace(r.ReviewText))
                .Select(r => r.ReviewText)
                .ToList();

            return await _openAIService.AskQuestionAboutReviewAsync(request.Question, reviewTexts);
        }
    }
}
