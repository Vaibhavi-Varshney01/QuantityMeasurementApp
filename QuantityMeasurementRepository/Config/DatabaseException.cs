namespace QuantityMeasurementRepository.Config
{
    public class DatabaseException : Exception
    {
        public string Operation { get; }

        public DatabaseException(string message, string operation = "UNKNOWN")
            : base(message)
        {
            Operation = operation;
        }

        public DatabaseException(string message, Exception inner,
            string operation = "UNKNOWN")
            : base(message, inner)
        {
            Operation = operation;
        }

        public override string ToString() =>
            InnerException == null
                ? $"[DatabaseException] Operation={Operation} | {Message}"
                : $"[DatabaseException] Operation={Operation} | {Message} | Inner={InnerException.GetType().Name}: {InnerException.Message}";
    }
}
