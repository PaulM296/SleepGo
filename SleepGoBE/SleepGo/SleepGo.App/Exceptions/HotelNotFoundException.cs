namespace SleepGo.App.Exceptions
{
    public class HotelNotFoundException : Exception
    {
        public HotelNotFoundException() { }
        public HotelNotFoundException(string message) : base(message) { }
        public HotelNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
