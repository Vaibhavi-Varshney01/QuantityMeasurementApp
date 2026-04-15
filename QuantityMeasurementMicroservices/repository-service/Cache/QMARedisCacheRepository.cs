using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RepositoryService.Interface;
using SharedModels.Entities;

namespace RepositoryService.Cache;

public sealed class QMARedisCacheRepository : IQMARepository
{
    private const string VersionKey = "qm:v1:version";
    private static readonly DistributedCacheEntryOptions DataOpts = new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10) };
    private static readonly DistributedCacheEntryOptions VerOpts  = new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(30) };
    private static readonly JsonSerializerOptions Jso = new() { PropertyNameCaseInsensitive = true };

    private readonly IQMARepository _inner;
    private readonly IDistributedCache _cache;
    public QMARedisCacheRepository(IQMARepository inner, IDistributedCache cache) { _inner = inner; _cache = cache; }

    public void Save(QuantityMeasurementEntity e) { _inner.Save(e); Bump(); }
    public List<QuantityMeasurementEntity> GetAll() => Cached("all", _inner.GetAll);
    public List<QuantityMeasurementEntity> GetByOperation(string op) => Cached($"op:{Uri.EscapeDataString(op)}", () => _inner.GetByOperation(op));
    public List<QuantityMeasurementEntity> GetByType(string t) => Cached($"type:{Uri.EscapeDataString(t)}", () => _inner.GetByType(t));
    public int GetTotalCount() { var k = Key("count"); if (TryGet(k, out int v)) return v; var c = _inner.GetTotalCount(); Set(k, c); return c; }
    public void DeleteAll() { _inner.DeleteAll(); Bump(); }

    private List<QuantityMeasurementEntity> Cached(string suffix, Func<List<QuantityMeasurementEntity>> fetch)
    {
        var k = Key(suffix);
        if (TryGet(k, out List<QuantityMeasurementEntity>? v)) return v!;
        var data = fetch(); Set(k, data); return data;
    }
    private string Key(string s) => $"qm:v1:{Ver()}:{s}";
    private string Ver() { try { var v = _cache.GetString(VersionKey); if (!string.IsNullOrWhiteSpace(v)) return v; v = Guid.NewGuid().ToString("N"); _cache.SetString(VersionKey, v, VerOpts); return v; } catch { return "x"; } }
    private void Bump() { try { _cache.SetString(VersionKey, Guid.NewGuid().ToString("N"), VerOpts); } catch { } }
    private bool TryGet<T>(string k, out T? val) { val = default; try { var j = _cache.GetString(k); if (string.IsNullOrEmpty(j)) return false; val = JsonSerializer.Deserialize<T>(j, Jso); return val != null; } catch { return false; } }
    private void Set<T>(string k, T v) { try { _cache.SetString(k, JsonSerializer.Serialize(v, Jso), DataOpts); } catch { } }
}
