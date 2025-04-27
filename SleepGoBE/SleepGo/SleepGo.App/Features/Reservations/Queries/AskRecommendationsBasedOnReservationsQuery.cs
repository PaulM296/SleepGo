using MediatR;
using SleepGo.App.Interfaces;

namespace SleepGo.App.Features.Reservations.Queries
{
    public record AskRecommendationsBasedOnReservationsQuery(Guid UserId) : IRequest<string>;

    public class AskRecommendationsBasedOnReservationsQueryHandler : IRequestHandler<AskRecommendationsBasedOnReservationsQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOpenAIService _openAIService;

        public AskRecommendationsBasedOnReservationsQueryHandler(IUnitOfWork unitOfWork, IOpenAIService openAIService)
        {
            _unitOfWork = unitOfWork;
            _openAIService = openAIService;
        }

        public async Task<string> Handle(AskRecommendationsBasedOnReservationsQuery request, CancellationToken cancellationToken)
        {
            var reservations = await _unitOfWork.ReservationRepository.GetAllReservationsByUserIdAsync(request.UserId);

            if(!reservations.Any())
            {
                return "You haven't made any reservations yet. Please make a reservation first!";
            }

            var pastStaySummaries = reservations.Select(r =>
                $"Hotel: {r.Room.Hotel.HotelName}, City: {r.Room.Hotel.City}, " +
                $"Country: {r.Room.Hotel.Country}, Room Type: {r.Room.RoomType}, " +
                $"Price Paid: {r.Price}, CheckIn: {r.CheckIn.ToShortDateString()}, CheckOut: {r.CheckOut.ToShortDateString()}"
            ).ToList();

            var prompt = $"Here is the reservation history of a user:\n\n{String.Join("\n", pastStaySummaries)}" +
                $"\n\nBased on these stays, recommend similar hotels, cities, or room types they might like next.";

            return await _openAIService.AskQuestionAboutHotelAsync("Based on my previous reservations, what would you recommend for my next hotel?", pastStaySummaries);
        }
    }
}
