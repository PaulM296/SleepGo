namespace SleepGo.App.Exceptions
{
    public class AmenityNotFoundException : Exception
    {
        public AmenityNotFoundException() { }
        public AmenityNotFoundException(string message) : base(message) { }
        public AmenityNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
