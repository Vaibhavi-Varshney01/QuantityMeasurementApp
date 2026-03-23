using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuantityMeasurementModel.Entities;

[Table("quantity_measurements")]
public class QuantityMeasurementEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string OperationType { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string MeasurementType { get; set; } = "Unknown";

    [MaxLength(200)]
    public string? Operand1 { get; set; }

    [MaxLength(200)]
    public string? Operand2 { get; set; }

    [MaxLength(200)]
    public string? Result { get; set; }

    public bool HasError { get; set; } = false;

    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public QuantityMeasurementEntity() { }

    public QuantityMeasurementEntity(string errorMessage)
    {
        OperationType = "ERROR";
        ErrorMessage = errorMessage ?? "Unknown error";
        HasError = true;
    }

    public QuantityMeasurementEntity(object operand1, string operationType, object result)
    {
        Operand1 = operand1?.ToString() ?? throw new ArgumentNullException(nameof(operand1));
        OperationType = operationType ?? throw new ArgumentNullException(nameof(operationType));
        Result = result?.ToString() ?? throw new ArgumentNullException(nameof(result));
        HasError = false;
    }

    public QuantityMeasurementEntity(object operand1, string operationType, object result, string measurementType)
        : this(operand1, operationType, result)
    {
        MeasurementType = measurementType ?? "Unknown";
    }

    public QuantityMeasurementEntity(object operand1, object operand2, string operationType, object result)
    {
        Operand1 = operand1?.ToString() ?? throw new ArgumentNullException(nameof(operand1));
        Operand2 = operand2?.ToString() ?? throw new ArgumentNullException(nameof(operand2));
        OperationType = operationType ?? throw new ArgumentNullException(nameof(operationType));
        Result = result?.ToString() ?? throw new ArgumentNullException(nameof(result));
        HasError = false;
    }

    public QuantityMeasurementEntity(object operand1, object operand2, string operationType, object result, string measurementType)
        : this(operand1, operand2, operationType, result)
    {
        MeasurementType = measurementType ?? "Unknown";
    }
}
