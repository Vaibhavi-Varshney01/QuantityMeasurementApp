using Npgsql;
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
        private readonly string _connectionString;

        public QuantityMeasurementDatabaseRepository(
            string connectionString, int poolSize = 5,
            ILogger<QuantityMeasurementDatabaseRepository>? logger = null)
        {
            _logger = logger ?? NullLogger<QuantityMeasurementDatabaseRepository>.Instance;
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _pool = new ConnectionPool(connectionString, poolSize);
            InitialiseSchema();
            _logger.LogInformation("[DatabaseRepository] Ready.");
        }

        // ── Schema init ───────────────────────────────────────────────────────
        private void InitialiseSchema()
        {
            NpgsqlConnection? conn = null;
            try
            {
                EnsureDatabaseExists();
                conn = _pool.Acquire();

                string sql = @"
                    CREATE TABLE IF NOT EXISTS QuantityMeasurements (
                        Id              SERIAL PRIMARY KEY,
                        OperationType   VARCHAR(50)  NOT NULL,
                        MeasurementType VARCHAR(50)  NOT NULL DEFAULT 'Unknown',
                        HasError        BOOLEAN      NOT NULL DEFAULT FALSE,
                        ErrorMessage    VARCHAR(500) NULL,
                        CreatedAt       TIMESTAMP      NOT NULL DEFAULT CURRENT_TIMESTAMP
                    );

                    CREATE TABLE IF NOT EXISTS QuantityMeasurementHistory (
                        Id            SERIAL PRIMARY KEY,
                        MeasurementId INT NOT NULL,
                        ChangedAt     TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
                        Note          VARCHAR(500) NULL,
                        FOREIGN KEY (MeasurementId) REFERENCES QuantityMeasurements(Id)
                    );

                    CREATE INDEX IF NOT EXISTS idx_operation_type   ON QuantityMeasurements(OperationType);
                    CREATE INDEX IF NOT EXISTS idx_measurement_type ON QuantityMeasurements(MeasurementType);";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.ExecuteNonQuery();
                Console.WriteLine("[DatabaseRepository] Schema verified.");
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Schema initialisation failed.", ex, "SCHEMA_INIT");
            }
            finally { if (conn != null) _pool.Release(conn); }
        }

        private void EnsureDatabaseExists()
        {
            var builder = new NpgsqlConnectionStringBuilder(_connectionString);
            string? databaseName = builder.Database;

            if (string.IsNullOrWhiteSpace(databaseName))
                return;

            // Connect to 'postgres' database to check/create the target database
            var masterBuilder = new NpgsqlConnectionStringBuilder(_connectionString)
            {
                Database = "postgres"
            };

            using var masterConn = new NpgsqlConnection(masterBuilder.ConnectionString);
            masterConn.Open();

            using var checkCmd = new NpgsqlCommand(
                "SELECT 1 FROM pg_database WHERE datname = @dbName", masterConn);
            checkCmd.Parameters.AddWithValue("@dbName", databaseName);
            var exists = checkCmd.ExecuteScalar() != null;

            if (!exists)
            {
                // Note: CREATE DATABASE cannot be executed in a transaction or with parameters for the DB name
                using var createCmd = new NpgsqlCommand(
                    $"CREATE DATABASE \"{databaseName.Replace("\"", "\"\"")}\"", masterConn);
                createCmd.ExecuteNonQuery();
            }
        }

        // ── Save ──────────────────────────────────────────────────────────────
        public void Save(QuantityMeasurementEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            NpgsqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                string sql = @"
                    INSERT INTO QuantityMeasurements
                        (OperationType, MeasurementType, HasError, ErrorMessage)
                    VALUES
                        (@op, @type, @hasError, @errorMsg)";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@op",       entity.OperationType);
                cmd.Parameters.AddWithValue("@type",     entity.MeasurementType);
                cmd.Parameters.AddWithValue("@hasError", entity.HasError);
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
            NpgsqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                var list = new List<QuantityMeasurementEntity>();
                using var cmd = new NpgsqlCommand(
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
            NpgsqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                var list = new List<QuantityMeasurementEntity>();
                using var cmd = new NpgsqlCommand(
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
            NpgsqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                var list = new List<QuantityMeasurementEntity>();
                using var cmd = new NpgsqlCommand(
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
            NpgsqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                using var cmd = new NpgsqlCommand(
                    "SELECT COUNT(*) FROM QuantityMeasurements", conn);
                return Convert.ToInt32(cmd.ExecuteScalar());
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
            NpgsqlConnection? conn = null;
            try
            {
                conn = _pool.Acquire();
                using var cmd = new NpgsqlCommand(
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
        private static QuantityMeasurementEntity MapRow(NpgsqlDataReader r)
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

