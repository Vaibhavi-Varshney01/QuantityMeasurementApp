using System;

namespace yard_equality
{
    public class InvalidUnitException : Exception
    {
        public InvalidUnitException(string message) : base(message)
        {
        }
    }
}