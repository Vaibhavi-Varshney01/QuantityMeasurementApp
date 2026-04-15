using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedModels.Entities;

[Table("quantity_measurements")]
public class QuantityMeasurementEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string OperationType { get; set; } = "Unknown";
    public string MeasurementType { get; set; } = "Unknown";
    public bool HasError { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? Operand1 { get; set; }
    public string? Operand2 { get; set; }
    public string? Result { get; set; }

    public QuantityMeasurementEntity() { }
    public QuantityMeasurementEntity(string operand1 = "", string operand2 = "",
        string operationType = "ERROR", string result = "", string measurementType = "Unknown")
    {
        Operand1 = operand1; Operand2 = operand2;
        OperationType = operationType; Result = result;
        MeasurementType = measurementType; HasError = false;
    }
    public QuantityMeasurementEntity(string errorMessage)
    {
        HasError = true; ErrorMessage = errorMessage; OperationType = "ERROR";
    }
}
