﻿namespace SleepGo.App.Exceptions
{
    public class ReservationNotFoundException : Exception
    {
        public ReservationNotFoundException() { }
        public ReservationNotFoundException(string message) : base(message) { }
        public ReservationNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
