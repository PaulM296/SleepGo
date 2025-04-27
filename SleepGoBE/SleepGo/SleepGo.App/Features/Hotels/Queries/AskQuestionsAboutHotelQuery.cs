using MediatR;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Hotels.Queries
{
    public record AskQuestionsAboutHotelQuery(string Question) : IRequest<string>;

    public class AskQuestionsAboutHotelQueryHandler : IRequestHandler<AskQuestionsAboutHotelQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOpenAIService _openAIService;

        public AskQuestionsAboutHotelQueryHandler(IUnitOfWork unitOfWork, IOpenAIService openAIService)
        {
            _unitOfWork = unitOfWork;
            _openAIService = openAIService;
        }

        public async Task<string> Handle(AskQuestionsAboutHotelQuery request, CancellationToken cancellationToken)
        {
            var hotels = await _unitOfWork.HotelRepository.GetAllHotelWithRoomsAsync();

            if(!hotels.Any())
            {
                return "There are no hotels available to recommend right now.";
            }

            var hotelSummaries = hotels.Select(hotel =>
            {
                var priceRange = hotel.Rooms != null && hotel.Rooms.Any()
                    ? $"Price Range: {hotel.Rooms.Min(r => r.Price)} to {hotel.Rooms.Max(r => r.Price)}"
                    : "Price Range: N/A";

                return $"Name: {hotel.HotelName}, City: {hotel.City}, Country: {hotel.Country}, Rating: {hotel.Rating}, {priceRange}";
            }).ToList();

            return await _openAIService.AskQuestionAboutHotelAsync(request.Question, hotelSummaries);
        }
    }
}
