using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel.DTO
{
    public class QuantityDTO
    {
        // [Required]
        public double Value { get; set; }
        // [Required]
        public string Unit { get; set; }
        // [Required]
        public string MeasurementType { get; set; } 
        public string OperationType { get; set; }
    }
}
