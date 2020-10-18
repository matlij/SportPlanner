using System;

namespace ModelsCore.Exceptions
{
    public class BadInputException : Exception
    {
        public BadInputException(string message) : base(message)
        {
        }
    }
}

