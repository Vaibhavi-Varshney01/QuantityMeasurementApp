using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.Config;
using QuantityMeasurementRepository.Connection;

namespace QuantityMeasurementRepository.Database
{
    public class QuantityMeasurementDatabaseRepository
        : IQuantityMeasurementRepository, IDisposable
    {
        private readonly ILogger<QuantityMeasurementDatabaseRepository> _logger;
        private readonly ConnectionPool _pool;

public QuantityMeasurementDatabaseRepository(
    string connectionString, int poolSize = 5,
    ILogger<QuantityMeasurementDatabaseRepository>? logger = null)
{
    _logger = logger ?? NullLogger<QuantityMeasurementDatabaseRepository>.Instance;
    _pool = new ConnectionPool(connectionString, poolSize);
    InitialiseSchema();
    _logger.LogInformation("[DatabaseRepository] Ready.");
}

        // ── Schema init ───────────────────────────────────────────────────────
        private void InitialiseSchema()
{
    SqlConnection? conn = null;
    try
    {
        conn = _pool.Acquire();

        string sql = @"
            IF NOT EXISTS (
                SELECT * FROM sys.tables WHERE name = 'QuantityMeasurements')
            BEGIN
                CREATE TABLE QuantityMeasurements (
                    Id              INT IDENTITY(1,1) PRIMARY KEY,
                    OperationType   NVARCHAR(50)  NOT NULL,
                    MeasurementType NVARCHAR(50)  NOT NULL DEFAULT 'Unknown',
                    HasError        BIT           NOT NULL DEFAULT 0,
                    ErrorMessage    NVARCHAR(500) NULL,
                    CreatedAt       DATETIME      NOT NULL DEFAULT GETDATE()
                )
            END

            IF NOT EXISTS (
                SELECT * FROM sys.tables WHERE name = 'QuantityMeasurementHistory')
            BEGIN
                CREATE TABLE QuantityMeasurementHistory (
                    Id            INT IDENTITY(1,1) PRIMARY KEY,
                    MeasurementId INT NOT NULL,
                    ChangedAt     DATETIME NOT NULL DEFAULT GETDATE(),
                    Note          NVARCHAR(500) NULL,
                    FOREIGN KEY (MeasurementId) REFERENCES QuantityMeasurements(Id)
                )
            END

            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_operation_type')
                CREATE INDEX idx_operation_type   ON QuantityMeasurements(OperationType)

            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'idx_measurement_type')
                CREATE INDEX idx_measurement_type ON QuantityMeasurements(MeasurementType)";

        using var cmd = new SqlCommand(sql, conn);
        cmd.ExecuteNonQuery();
        Console.WriteLine("[DatabaseRepository] Schema verified.");
    }
    catch (Exception ex)
    {
        throw new DatabaseException("Schema initialisation failed.", ex, "SCHEMA_INIT");
    }
    finally { if (conn != null) _pool.Release(conn); }
}

        // ── Save ──────────────────────────────────────────────────────────────
        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                string sql = @"
                    INSERT INTO QuantityMeasurements
                        (OperationType, MeasurementType, HasError, ErrorMessage)
                    VALUES
                        (@op, @type, @hasError, @errorMsg)";
                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@op",       entity.OperationType);
                cmd.Parameters.AddWithValue("@type",     entity.MeasurementType);
                cmd.Parameters.AddWithValue("@hasError", entity.HasError ? 1 : 0);
                cmd.Parameters.AddWithValue("@errorMsg",
                    (object?)entity.ErrorMessage ?? DBNull.Value);
                cmd.ExecuteNonQuery();
                Console.WriteLine(
                    $"[DatabaseRepository] Saved: {entity.OperationType} | {entity.MeasurementType}");
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Save failed.", ex, "SAVE");
            }
            finally { if (conn != null) _pool.Release(conn); }
        }

        // ── GetAllMeasurements ────────────────────────────────────────────────
        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                var list = new List<QuantityMeasurementEntity>();
                using var cmd = new SqlCommand(
                    "SELECT * FROM QuantityMeasurements ORDER BY CreatedAt DESC", conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(MapRow(reader));
                return list;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("GetAllMeasurements failed.", ex, "GET_ALL");
            }
            finally { if (conn != null) _pool.Release(conn); }
        }

        // ── GetMeasurementsByOperation ────────────────────────────────────────
        public List<QuantityMeasurementEntity> GetMeasurementsByOperation(
            string operationType)
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                var list = new List<QuantityMeasurementEntity>();
                using var cmd = new SqlCommand(
                    "SELECT * FROM QuantityMeasurements WHERE OperationType = @op",
                    conn);
                cmd.Parameters.AddWithValue("@op", operationType);
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(MapRow(reader));
                return list;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    $"GetByOperation failed for '{operationType}'.", ex, "GET_BY_OP");
            }
            finally { if (conn != null) _pool.Release(conn); }
        }

        // ── GetMeasurementsByType ─────────────────────────────────────────────
        public List<QuantityMeasurementEntity> GetMeasurementsByType(
            string measurementType)
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                var list = new List<QuantityMeasurementEntity>();
                using var cmd = new SqlCommand(
                    "SELECT * FROM QuantityMeasurements WHERE MeasurementType = @type",
                    conn);
                cmd.Parameters.AddWithValue("@type", measurementType);
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) list.Add(MapRow(reader));
                return list;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    $"GetByType failed for '{measurementType}'.", ex, "GET_BY_TYPE");
            }
            finally { if (conn != null) _pool.Release(conn); }
        }

        // ── GetTotalCount ─────────────────────────────────────────────────────
        public int GetTotalCount()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                using var cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM QuantityMeasurements", conn);
                return (int)cmd.ExecuteScalar()!;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("GetTotalCount failed.", ex, "COUNT");
            }
            finally { if (conn != null) _pool.Release(conn); }
        }

        // ── DeleteAll ─────────────────────────────────────────────────────────
        public void DeleteAll()
        {
            SqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                using var cmd = new SqlCommand(
                    "DELETE FROM QuantityMeasurements", conn);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine($"[DatabaseRepository] Deleted {rows} row(s).");
            }
            catch (Exception ex)
            {
                throw new DatabaseException("DeleteAll failed.", ex, "DELETE_ALL");
            }
            finally { if (conn != null) _pool.Release(conn); }
        }

        // ── Pool helpers ──────────────────────────────────────────────────────
        public string GetPoolStatistics() => _pool.GetStatistics();

        public void ReleaseResources() => _pool.Dispose();

        public void Dispose() => _pool.Dispose();

        // ── Map DB row → entity ───────────────────────────────────────────────
        private static QuantityMeasurementEntity MapRow(SqlDataReader r)
        {
            string opType  = r.GetString(r.GetOrdinal("OperationType"));
            string measType = r.GetString(r.GetOrdinal("MeasurementType"));
            bool hasError  = r.GetBoolean(r.GetOrdinal("HasError"));

            QuantityMeasurementEntity entity;
            if (hasError)
            {
                string errMsg = r.IsDBNull(r.GetOrdinal("ErrorMessage"))
                    ? "Unknown error"
                    : r.GetString(r.GetOrdinal("ErrorMessage"));
                entity = new QuantityMeasurementEntity(errMsg);
            }
            else
            {
                entity = new QuantityMeasurementEntity(opType, "LOAD", opType);
            }

            entity.MeasurementType = measType;
            return entity;
        }
    }
}