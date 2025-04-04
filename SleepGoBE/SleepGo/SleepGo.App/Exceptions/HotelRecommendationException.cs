namespace SleepGo.App.Exceptions
{
    public class HotelRecommendationException : Exception
    {
        public HotelRecommendationException() { }
        public HotelRecommendationException(string message) : base(message) { }
        public HotelRecommendationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
