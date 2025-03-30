using SleepGo.Domain.Enums;
using System.Numerics;

namespace SleepGo.MLTrainer.Models
{
    public class HotelRecommendationData
    {
        public string UserId {  get; set; }
        public string HotelId { get; set;}
        public float HotelRating { get; set; }
        public float PricePaid { get; set; }
        public float Label { get; set; }  // Needed for ML.NET
        public string City { get; set; }
        public string Country { get; set; }
        public RoomType RoomType { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
    }
}
