using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementRepository
{
    public sealed class QuantityMeasurementJsonCacheRepository : IQuantityMeasurementRepository
    {
        private readonly string _filePath;
        private readonly object _gate = new object();

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        private sealed class CacheItem
        {
            public string? Operand1 { get; set; }
            public string? Operand2 { get; set; }
            public string OperationType { get; set; } = "Unknown";
            public string MeasurementType { get; set; } = "Unknown";
            public string? Result { get; set; }
            public bool HasError { get; set; }
            public string? ErrorMessage { get; set; }
            public DateTimeOffset CreatedAtUtc { get; set; }
        }

        public QuantityMeasurementJsonCacheRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException("File path must be provided.", nameof(filePath));

            _filePath = Path.GetFullPath(filePath);
            EnsureFileExists();
            Console.WriteLine($"[JsonCacheRepository] Ready. File: {_filePath}");
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            lock (_gate)
            {
                var items = ReadItemsUnsafe();
                items.Add(new CacheItem
                {
                    Operand1 = entity.Operand1?.ToString(),
                    Operand2 = entity.Operand2?.ToString(),
                    OperationType = entity.OperationType,
                    MeasurementType = entity.MeasurementType,
                    Result = entity.Result?.ToString(),
                    HasError = entity.HasError,
                    ErrorMessage = entity.ErrorMessage,
                    CreatedAtUtc = DateTimeOffset.UtcNow
                });
                WriteItemsUnsafe(items);
            }
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            lock (_gate)
            {
                return ReadItemsUnsafe().Select(MapToEntity).ToList();
            }
        }

        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(string operationType)
        {
            if (operationType == null) throw new ArgumentNullException(nameof(operationType));
            lock (_gate)
            {
                return ReadItemsUnsafe()
                    .Where(i => i.OperationType.Equals(operationType, StringComparison.OrdinalIgnoreCase))
                    .Select(MapToEntity)
                    .ToList();
            }
        }

        public List<QuantityMeasurementEntity> GetMeasurementsByType(string measurementType)
        {
            if (measurementType == null) throw new ArgumentNullException(nameof(measurementType));
            lock (_gate)
            {
                return ReadItemsUnsafe()
                    .Where(i => i.MeasurementType.Equals(measurementType, StringComparison.OrdinalIgnoreCase))
                    .Select(MapToEntity)
                    .ToList();
            }
        }

        public int GetTotalCount()
        {
            lock (_gate)
            {
                return ReadItemsUnsafe().Count;
            }
        }

        public void DeleteAll()
        {
            lock (_gate)
            {
                WriteItemsUnsafe(new List<CacheItem>());
            }
            Console.WriteLine("[JsonCacheRepository] All records cleared.");
        }

        private void EnsureFileExists()
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(directory))
                Directory.CreateDirectory(directory);

            if (!File.Exists(_filePath))
                File.WriteAllText(_filePath, "[]");
        }

        private List<CacheItem> ReadItemsUnsafe()
        {
            EnsureFileExists();
            string json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<CacheItem>>(json, JsonOptions) ?? new List<CacheItem>();
        }

        private void WriteItemsUnsafe(List<CacheItem> items)
        {
            EnsureFileExists();
            string tmpPath = _filePath + ".tmp";
            File.WriteAllText(tmpPath, JsonSerializer.Serialize(items, JsonOptions));
            File.Replace(tmpPath, _filePath, destinationBackupFileName: null);
        }

        private static QuantityMeasurementEntity MapToEntity(CacheItem item)
        {
            if (item.HasError || item.OperationType.Equals("ERROR", StringComparison.OrdinalIgnoreCase))
            {
                var err = new QuantityMeasurementEntity(item.ErrorMessage ?? "Unknown error");
                err.MeasurementType = item.MeasurementType;
                return err;
            }

            QuantityMeasurementEntity entity = string.IsNullOrWhiteSpace(item.Operand2)
                ? new QuantityMeasurementEntity(item.Operand1 ?? string.Empty, item.OperationType, item.Result ?? string.Empty)
                : new QuantityMeasurementEntity(
                    operand1: item.Operand1 ?? string.Empty,
                    operand2: item.Operand2 ?? string.Empty,
                    operationType: item.OperationType,
                    result: item.Result ?? string.Empty);

            entity.MeasurementType = item.MeasurementType;
            return entity;
        }
    }
}
