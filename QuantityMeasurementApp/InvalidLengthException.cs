using System;

namespace yard_equality
{
    public class InvalidLengthException : Exception
    {
        public InvalidLengthException(string message) : base(message)
        {
        }
    }
}