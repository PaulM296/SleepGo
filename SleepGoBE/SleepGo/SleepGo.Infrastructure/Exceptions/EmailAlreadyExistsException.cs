﻿namespace SleepGo.Infrastructure.Exceptions
{
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException() { }

        public EmailAlreadyExistsException(string message) : base(message) { }

        public EmailAlreadyExistsException(string message, Exception innerException) : base(message, innerException) { }
    }
}
