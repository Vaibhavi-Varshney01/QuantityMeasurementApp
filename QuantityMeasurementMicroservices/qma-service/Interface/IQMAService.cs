using SharedModels.DTO;

namespace QMAService.Interface;

public interface IQMAService
{
    Task<QuantityMeasurementDTO> Compare(QuantityDTO q1, QuantityDTO q2);
    Task<QuantityMeasurementDTO> Convert(QuantityDTO q, string targetUnit);
    Task<QuantityMeasurementDTO> Add(QuantityDTO q1, QuantityDTO q2);
    Task<QuantityMeasurementDTO> Subtract(QuantityDTO q1, QuantityDTO q2);
    Task<QuantityMeasurementDTO> Divide(QuantityDTO q1, QuantityDTO q2);
    Task<List<QuantityMeasurementDTO>> GetHistoryByOperation(string operation);
    Task<List<QuantityMeasurementDTO>> GetHistoryByType(string type);
    Task<List<QuantityMeasurementDTO>> GetErrorHistory();
    Task<int> CountByOperation(string operation);
}
