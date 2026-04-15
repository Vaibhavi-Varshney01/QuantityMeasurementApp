using System.Net.Http.Json;
using QMAService.Interface;
using SharedModels.DTO;
using SharedModels.Entities;

namespace QMAService.Service;

/// <summary>
/// Business layer — calls repository-service over HTTP for persistence.
/// </summary>
public class QMAServiceImpl : IQMAService
{
    private readonly HttpClient _http;
    public QMAServiceImpl(IHttpClientFactory factory) => _http = factory.CreateClient("repository");

    public async Task<QuantityMeasurementDTO> Compare(QuantityDTO q1, QuantityDTO q2)
    {
        bool equal = Math.Abs(q1.Value - q2.Value) < 1e-9 &&
                     string.Equals(q1.Unit, q2.Unit, StringComparison.OrdinalIgnoreCase);

        await SaveAsync(new QuantityMeasurementEntity(
            $"{q1.Value} {q1.Unit}", $"{q2.Value} {q2.Unit}",
            "COMPARE", equal.ToString(), q1.MeasurementType));

        return new QuantityMeasurementDTO
        {
            ThisValue = q1.Value, ThisUnit = q1.Unit, ThisMeasurementType = q1.MeasurementType,
            ThatValue = q2.Value, ThatUnit = q2.Unit, ThatMeasurementType = q2.MeasurementType,
            Operation = "COMPARE", ResultString = equal.ToString(), IsError = false
        };
    }

    public async Task<QuantityMeasurementDTO> Convert(QuantityDTO q, string targetUnit)
    {
        await SaveAsync(new QuantityMeasurementEntity(
            $"{q.Value} {q.Unit}", "", "CONVERT", $"{q.Value} {targetUnit}", q.MeasurementType));

        return new QuantityMeasurementDTO
        {
            ThisValue = q.Value, ThisUnit = q.Unit, ThisMeasurementType = q.MeasurementType,
            Operation = "CONVERT", ResultString = $"{q.Value} {targetUnit}", IsError = false
        };
    }

    public async Task<QuantityMeasurementDTO> Add(QuantityDTO q1, QuantityDTO q2)
    {
        double result = q1.Value + q2.Value;
        await SaveAsync(new QuantityMeasurementEntity(
            $"{q1.Value} {q1.Unit}", $"{q2.Value} {q2.Unit}", "ADD",
            $"{result} {q1.Unit}", q1.MeasurementType));

        return new QuantityMeasurementDTO
        {
            ThisValue = q1.Value, ThisUnit = q1.Unit, ThisMeasurementType = q1.MeasurementType,
            ThatValue = q2.Value, ThatUnit = q2.Unit, ThatMeasurementType = q2.MeasurementType,
            Operation = "ADD", ResultValue = result, ResultUnit = q1.Unit,
            ResultMeasurementType = q1.MeasurementType, IsError = false
        };
    }

    public async Task<QuantityMeasurementDTO> Subtract(QuantityDTO q1, QuantityDTO q2)
    {
        double result = q1.Value - q2.Value;
        await SaveAsync(new QuantityMeasurementEntity(
            $"{q1.Value} {q1.Unit}", $"{q2.Value} {q2.Unit}", "SUBTRACT",
            $"{result} {q1.Unit}", q1.MeasurementType));

        return new QuantityMeasurementDTO
        {
            ThisValue = q1.Value, ThisUnit = q1.Unit, ThisMeasurementType = q1.MeasurementType,
            ThatValue = q2.Value, ThatUnit = q2.Unit, ThatMeasurementType = q2.MeasurementType,
            Operation = "SUBTRACT", ResultValue = result, ResultUnit = q1.Unit,
            ResultMeasurementType = q1.MeasurementType, IsError = false
        };
    }

    public async Task<QuantityMeasurementDTO> Divide(QuantityDTO q1, QuantityDTO q2)
    {
        if (Math.Abs(q2.Value) < 1e-12)
        {
            await SaveAsync(new QuantityMeasurementEntity("Division by zero"));
            return new QuantityMeasurementDTO { Operation = "DIVIDE", ErrorMessage = "Division by zero", IsError = true };
        }
        double result = q1.Value / q2.Value;
        await SaveAsync(new QuantityMeasurementEntity(
            $"{q1.Value} {q1.Unit}", $"{q2.Value} {q2.Unit}", "DIVIDE",
            result.ToString(), q1.MeasurementType));

        return new QuantityMeasurementDTO
        {
            ThisValue = q1.Value, ThisUnit = q1.Unit, ThisMeasurementType = q1.MeasurementType,
            ThatValue = q2.Value, ThatUnit = q2.Unit, ThatMeasurementType = q2.MeasurementType,
            Operation = "DIVIDE", ResultValue = result, IsError = false
        };
    }

    public async Task<List<QuantityMeasurementDTO>> GetHistoryByOperation(string op)
    {
        var entities = await _http.GetFromJsonAsync<List<QuantityMeasurementEntity>>(
            $"internal/measurements/operation/{op}") ?? new();
        return Map(entities);
    }

    public async Task<List<QuantityMeasurementDTO>> GetHistoryByType(string type)
    {
        var entities = await _http.GetFromJsonAsync<List<QuantityMeasurementEntity>>(
            $"internal/measurements/type/{type}") ?? new();
        return Map(entities);
    }

    public async Task<List<QuantityMeasurementDTO>> GetErrorHistory()
    {
        var entities = await _http.GetFromJsonAsync<List<QuantityMeasurementEntity>>(
            "internal/measurements/errors") ?? new();
        return Map(entities);
    }

    public async Task<int> CountByOperation(string op)
    {
        var result = await _http.GetFromJsonAsync<int>($"internal/measurements/count/{op}");
        return result;
    }

    private async Task SaveAsync(QuantityMeasurementEntity entity)
        => await _http.PostAsJsonAsync("internal/measurements", entity);

    private static List<QuantityMeasurementDTO> Map(List<QuantityMeasurementEntity> entities)
        => entities.Select(e => new QuantityMeasurementDTO
        {
            Operation = e.OperationType,
            ThisMeasurementType = e.MeasurementType,
            ResultString = e.Result,
            ErrorMessage = e.ErrorMessage,
            IsError = e.HasError
        }).ToList();
}
