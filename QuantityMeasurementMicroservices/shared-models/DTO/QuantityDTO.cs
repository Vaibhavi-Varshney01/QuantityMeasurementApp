namespace SharedModels.DTO;
public class QuantityDTO
{
    public double Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string MeasurementType { get; set; } = string.Empty;
    public string OperationType { get; set; } = string.Empty;
}
