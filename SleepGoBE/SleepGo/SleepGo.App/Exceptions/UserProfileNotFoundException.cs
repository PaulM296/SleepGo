namespace SleepGo.App.Exceptions
{
    public class UserProfileNotFoundException : Exception
    {
        public UserProfileNotFoundException() : base() { }
        public UserProfileNotFoundException(string message) : base(message) { }
        public UserProfileNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
