using Microsoft.ML;
using SleepGo.Api.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.MLTrainer.Models;

namespace SleepGo.Api.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _model;
        private readonly PredictionEngine<HotelRecommendationData, HotelRecommendationPrediction> _predictionEngine;

        public RecommendationService(IWebHostEnvironment env)
        {
            _mlContext = new MLContext();

            var modelPath = Path.Combine(env.ContentRootPath, "MLModels", "hotel_recommendation_mode.zip");
            _model = _mlContext.Model.Load(modelPath, out _);
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<HotelRecommendationData, HotelRecommendationPrediction>(_model);
        }

        public HotelRecommendationPrediction Predict(HotelRecommendationData input)
        {
            return _predictionEngine.Predict(input);
        }

        public IEnumerable<(HotelRecommendationData Hotel, HotelRecommendationPrediction Prediction, string HotelName)> RecommendTopHotels(Guid userId, List<Room> rooms)
        {
            var results = new List<(HotelRecommendationData Hotel, HotelRecommendationPrediction Prediction, string HotelName)>();


            foreach (var room in rooms)
            {
                var input = new HotelRecommendationData
                {
                    UserId = userId.ToString(),
                    HotelId = room.HotelId.ToString(),
                    HotelRating = (float)room.Hotel.Rating,
                    PricePaid = (float)room.Price,
                    City = room.Hotel.City,
                    Country = room.Hotel.Country,
                    RoomType = (int)room.RoomType
                };

                var prediction = _predictionEngine.Predict(input);
                results.Add((input, prediction, room.Hotel.HotelName));
            }

            return results.OrderByDescending(r => r.Prediction.Probability).Take(5);
        }
    }
}
