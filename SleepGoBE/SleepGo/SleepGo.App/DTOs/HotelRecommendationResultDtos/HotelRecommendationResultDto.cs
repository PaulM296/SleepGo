namespace SleepGo.App.DTOs.HotelRecommendationResultDtos
{
    public class HotelRecommendationResultDto
    {
        public Guid HotelId { get; set; }
        public string HotelName { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public decimal Price { get; set; }
        public bool PredictedLabel { get; set; }
        public float Probability { get; set; }
    }
}
