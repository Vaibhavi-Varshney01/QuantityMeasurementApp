using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository.Cache;

public sealed class QuantityMeasurementRedisCacheRepository : IQuantityMeasurementRepository
{
    private const string VersionKey = "qm:cache:v1:version";
    private static readonly DistributedCacheEntryOptions DefaultDataCacheOptions =
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };

    private static readonly DistributedCacheEntryOptions VersionCacheOptions =
        new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30)
        };

    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly IQuantityMeasurementRepository _inner;
    private readonly IDistributedCache _cache;

    public QuantityMeasurementRedisCacheRepository(
        IQuantityMeasurementRepository inner,
        IDistributedCache cache)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public void Save(QuantityMeasurementEntity entity)
    {
        _inner.Save(entity);
        BumpVersionSafe();
    }

    public List<QuantityMeasurementEntity> GetAllMeasurements()
    {
        string key = GetKey("all");
        if (TryGet(key, out List<QuantityMeasurementEntity>? cached))
            return cached!;

        var data = _inner.GetAllMeasurements();
        SetSafe(key, data);
        return data;
    }

    public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
    {
        string key = GetKey($"op:{Escape(operationType)}");
        if (TryGet(key, out List<QuantityMeasurementEntity>? cached))
            return cached!;

        var data = _inner.GetMeasurementsByOperation(operationType);
        SetSafe(key, data);
        return data;
    }

    public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType)
    {
        string key = GetKey($"type:{Escape(measurementType)}");
        if (TryGet(key, out List<QuantityMeasurementEntity>? cached))
            return cached!;

        var data = _inner.GetMeasurementsByType(measurementType);
        SetSafe(key, data);
        return data;
    }

    public int GetTotalCount()
    {
        string key = GetKey("count");
        if (TryGet(key, out int cached))
            return cached;

        int count = _inner.GetTotalCount();
        SetSafe(key, count);
        return count;
    }

    public void DeleteAll()
    {
        _inner.DeleteAll();
        BumpVersionSafe();
    }

    public string GetPoolStatistics() => _inner.GetPoolStatistics();

    public void ReleaseResources() => _inner.ReleaseResources();

    private string GetKey(string suffix) => $"qm:cache:v1:{GetVersionSafe()}:{suffix}";

    private static string Escape(string value) => Uri.EscapeDataString(value ?? string.Empty);

    private string GetVersionSafe()
    {
        try
        {
            string? v = _cache.GetString(VersionKey);
            if (!string.IsNullOrWhiteSpace(v))
                return v;

            v = Guid.NewGuid().ToString("N");
            _cache.SetString(VersionKey, v, VersionCacheOptions);
            return v;
        }
        catch
        {
            return "no-redis";
        }
    }

    private void BumpVersionSafe()
    {
        try
        {
            _cache.SetString(VersionKey, Guid.NewGuid().ToString("N"), VersionCacheOptions);
        }
        catch
        {
            // ignore cache failures; DB remains source of truth
        }
    }

    private bool TryGet<T>(string key, out T? value)
    {
        value = default;
        try
        {
            string? json = _cache.GetString(key);
            if (string.IsNullOrWhiteSpace(json))
                return false;

            value = JsonSerializer.Deserialize<T>(json, JsonOptions);
            return value != null;
        }
        catch
        {
            return false;
        }
    }

    private void SetSafe<T>(string key, T value)
    {
        try
        {
            _cache.SetString(key, JsonSerializer.Serialize(value, JsonOptions), DefaultDataCacheOptions);
        }
        catch
        {
            // ignore cache failures; DB remains source of truth
        }
    }
}
