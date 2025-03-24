namespace SleepGo.App.Exceptions
{
    public class PaymentIntentNotFoundException : Exception
    {
        public PaymentIntentNotFoundException() { }
        public PaymentIntentNotFoundException(string message) : base(message) { }
        public PaymentIntentNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
