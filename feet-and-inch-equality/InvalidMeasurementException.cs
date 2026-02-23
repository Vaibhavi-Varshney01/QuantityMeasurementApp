using System;

namespace feet_and_inches_equality
{
    public class InvalidMeasurementException : Exception
    {
        public InvalidMeasurementException(string message) : base(message) { }
    }
}