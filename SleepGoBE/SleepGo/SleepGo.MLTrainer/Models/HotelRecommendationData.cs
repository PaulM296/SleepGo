using Microsoft.ML.Data;

namespace SleepGo.MLTrainer.Models
{
    public class HotelRecommendationData
    {
        [LoadColumn(0)]
        public string UserId {  get; set; }
        [LoadColumn(1)]
        public string HotelId { get; set;}
        [LoadColumn(2)]
        public float HotelRating { get; set; }
        [LoadColumn(3)]
        public float PricePaid { get; set; }
        [LoadColumn(4)]
        public bool Label { get; set; }  // Needed for ML.NET
        [LoadColumn(5)]
        public string City { get; set; }
        [LoadColumn(6)]
        public string Country { get; set; }
        [LoadColumn(7)]
        public int RoomType { get; set; }
        //[LoadColumn(8)]
        //public DateTime Checkin { get; set; }
        //[LoadColumn(9)]
        //public DateTime Checkout { get; set; }
    }
}
