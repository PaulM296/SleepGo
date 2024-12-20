namespace SleepGo.App.Exceptions
{
    public class InvalidImageFormatException : Exception
    {
        public InvalidImageFormatException() : base() { }
        public InvalidImageFormatException(string message) : base(message) { }
        public InvalidImageFormatException(string message, Exception innerException) : base(message, innerException) { }
    }
}
