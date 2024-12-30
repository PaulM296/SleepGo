namespace SleepGo.App.Exceptions
{
    public class NoAvailableRoomsException : Exception
    {
        public NoAvailableRoomsException() { }
        public NoAvailableRoomsException(string message) : base(message) { }
        public NoAvailableRoomsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
