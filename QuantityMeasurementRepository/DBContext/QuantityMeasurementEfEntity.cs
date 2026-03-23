using System;

namespace QuantityMeasurementRepository
{
    public class QuantityMeasurementEfEntity
    {
        public int Id { get; set; }
        public string OperationType { get; set; } = "Unknown";
        public string MeasurementType { get; set; } = "Unknown";
        public bool HasError { get; set; }
        public string? ErrorMessage { get; set; }
        public DateTime CreatedAt { get; set; }

        public string? Operand1 { get; set; }
        public string? Operand2 { get; set; }
        public string? Result { get; set; }
    }
}

