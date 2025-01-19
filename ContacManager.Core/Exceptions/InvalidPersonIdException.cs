﻿namespace ContacManager.Core.Exceptions
{
    public class InvalidPersonIdException : ArgumentException
    {
        public InvalidPersonIdException() : base()
        {

        }
        public InvalidPersonIdException(string? message) : base(message)
        {

        }
        public InvalidPersonIdException(string? message, Exception innerException)
        {

        }

    }
}
