namespace QuantityMeasurementBusinessLayer.DTO;

public class QuantityMeasurementDTO
{
    public double ThisValue { get; set; }
    public string? ThisUnit { get; set; }
    public string? ThisMeasurementType { get; set; }

    public double ThatValue { get; set; }
    public string? ThatUnit { get; set; }
    public string? ThatMeasurementType { get; set; }

    public string? Operation { get; set; }
    public string? ResultString { get; set; }
    public double ResultValue { get; set; }
    public string? ResultUnit { get; set; }
    public string? ResultMeasurementType { get; set; }

    public string? ErrorMessage { get; set; }
    public bool IsError { get; set; }
}