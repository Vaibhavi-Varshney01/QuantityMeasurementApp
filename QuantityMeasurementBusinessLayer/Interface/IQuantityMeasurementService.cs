using QuantityMeasurementModel.DTO;

namespace QuantityMeasurementBusinessLayer;

public interface IQuantityMeasurementService
{
    QuantityMeasurementDTO Compare(QuantityDTO q1, QuantityDTO q2);
    QuantityMeasurementDTO Convert(QuantityDTO q, string targetUnit);
    QuantityMeasurementDTO Add(QuantityDTO q1, QuantityDTO q2);
    QuantityMeasurementDTO Subtract(QuantityDTO q1, QuantityDTO q2);
    QuantityMeasurementDTO Divide(QuantityDTO q1, QuantityDTO q2);

    List<QuantityMeasurementDTO> GetHistoryByOperation(string operation);
    List<QuantityMeasurementDTO> GetHistoryByType(string type);
    List<QuantityMeasurementDTO> GetErrorHistory();
    int CountByOperation(string operation);
}


