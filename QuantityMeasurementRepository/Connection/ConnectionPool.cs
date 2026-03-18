using Microsoft.Data.SqlClient;

namespace QuantityMeasurementRepository.Connection
{
    public class ConnectionPool : IDisposable
    {
        private readonly string _connectionString;
        private readonly int _maxSize;
        private readonly Queue<SqlConnection> _available;
        private int _totalCreated;
        private bool _disposed;
        private readonly object _lock = new();

        public ConnectionPool(string connectionString, int maxSize = 5)
        {
            _connectionString = connectionString;
            _maxSize = maxSize;
            _available = new Queue<SqlConnection>();
            Console.WriteLine($"[ConnectionPool] Initialised (max={_maxSize}).");
        }

        public SqlConnection Acquire()
        {
            lock (_lock)
            {
                while (_available.Count > 0)
                {
                    var conn = _available.Dequeue();
                    if (conn.State == System.Data.ConnectionState.Open)
                        return conn;
                    conn.Dispose();
                    _totalCreated--;
                }

                if (_totalCreated >= _maxSize)
                    throw new InvalidOperationException(
                        $"Connection pool exhausted (max={_maxSize}).");

                var newConn = new SqlConnection(_connectionString);
                newConn.Open();
                _totalCreated++;
                Console.WriteLine($"[ConnectionPool] New connection ({_totalCreated}/{_maxSize}).");
                return newConn;
            }
        }

        public void Release(SqlConnection connection)
        {
            if (connection == null) return;
            lock (_lock)
            {
                if (connection.State == System.Data.ConnectionState.Open)
                    _available.Enqueue(connection);
                else
                {
                    connection.Dispose();
                    _totalCreated--;
                }
            }
        }

        public string GetStatistics()
        {
            lock (_lock)
            {
                int inUse = _totalCreated - _available.Count;
                return $"Pool — available: {_available.Count}, " +
                       $"in-use: {inUse}, total: {_totalCreated}/{_maxSize}";
            }
        }

        public void Dispose()
        {
            if (_disposed) return;
            lock (_lock)
            {
                while (_available.Count > 0)
                    _available.Dequeue().Dispose();
                _disposed = true;
                Console.WriteLine("[ConnectionPool] Disposed.");
            }
        }
    }
}