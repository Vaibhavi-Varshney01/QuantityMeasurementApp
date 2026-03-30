using QuantityMeasurementModel.DTO;

namespace QuantityMeasurementBusinessLayer;

/// <summary>
/// Defines the core quantity comparison, conversion, and history helpers.
/// </summary>
public interface IQuantityMeasurementService
{
    /// <summary>
    /// Compare two quantities and return a DTO that records the relationship.
    /// </summary>
    QuantityMeasurementDTO Compare(QuantityDTO q1, QuantityDTO q2);

    /// <summary>
    /// Convert a source quantity into the requested target unit.
    /// </summary>
    QuantityMeasurementDTO Convert(QuantityDTO q, string targetUnit);

    /// <summary>
    /// Sum two quantities (must share compatible units before addition).
    /// </summary>
    QuantityMeasurementDTO Add(QuantityDTO q1, QuantityDTO q2);

    /// <summary>
    /// Subtract one quantity from another and express the result in a common unit.
    /// </summary>
    QuantityMeasurementDTO Subtract(QuantityDTO q1, QuantityDTO q2);

    /// <summary>
    /// Divide one stored quantity by another and capture the numeric result.
    /// </summary>
    QuantityMeasurementDTO Divide(QuantityDTO q1, QuantityDTO q2);

    /// <summary>
    /// Pull a list of past calculations filtered by the requested operation name.
    /// </summary>
    List<QuantityMeasurementDTO> GetHistoryByOperation(string operation);

    /// <summary>
    /// Pull a history page filtered by the measurement type (for example length, volume, etc.).
    /// </summary>
    List<QuantityMeasurementDTO> GetHistoryByType(string type);

    /// <summary>
    /// Retrieve all history entries that failed validation or comparison rules.
    /// </summary>
    List<QuantityMeasurementDTO> GetErrorHistory();

    /// <summary>
    /// Count how often a specific operation has been performed for reporting.
    /// </summary>
    int CountByOperation(string operation);
}
