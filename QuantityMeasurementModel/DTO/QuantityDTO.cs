using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel.DTO
{
    public class QuantityDTO
    {
        public double Value { get; set; }
        public string? Unit { get; set; }
        public string? MeasurementType { get; set; } 
        public string? OperationType { get; set; }
    }
}
