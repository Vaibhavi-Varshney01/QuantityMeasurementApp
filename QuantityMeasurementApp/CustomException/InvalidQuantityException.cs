namespace QuantityMeasurementApp.CustomException
{
    public class InvalidQuantityException : System.Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }
}