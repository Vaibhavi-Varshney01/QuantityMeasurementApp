using System.Text.Json;

namespace QuantityMeasurementRepository.Config
{
    public class AppConfig
    {
        private static AppConfig? _instance;
        private readonly Dictionary<string, string> _props = new();

        private AppConfig()
        {
            LoadFromFile();
        }

        public static AppConfig Instance => _instance ??= new AppConfig();

        private void LoadFromFile()
        {
            // Look for appsettings.json next to the running executable
            string path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(path))
            {
                Console.WriteLine("[AppConfig] appsettings.json not found — using defaults.");
                SetDefaults();
                return;
            }

            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var root = doc.RootElement;

            if (root.TryGetProperty("Database", out var db))
            {
                if (db.TryGetProperty("ConnectionString", out var cs))
                    _props["db.connectionString"] = cs.GetString()!;
                if (db.TryGetProperty("RepositoryType", out var rt))
                    _props["db.repositoryType"] = rt.GetString()!;
                if (db.TryGetProperty("MaxPoolSize", out var ps))
                    _props["db.maxPoolSize"] = ps.GetInt32().ToString();
            }

            Console.WriteLine("[AppConfig] Configuration loaded successfully.");
        }

        private void SetDefaults()
        {
            _props["db.connectionString"] =
                "Server=localhost\\SQLEXPRESS;Database=QuantityDB;" +
                "Trusted_Connection=True;TrustServerCertificate=True;";
            _props["db.repositoryType"] = "cache";
            _props["db.maxPoolSize"] = "5";
        }

        public string GetConnectionString() =>
            _props.GetValueOrDefault("db.connectionString", "");

        public string GetRepositoryType() =>
            _props.GetValueOrDefault("db.repositoryType", "cache");

        public int GetMaxPoolSize() =>
            int.Parse(_props.GetValueOrDefault("db.maxPoolSize", "5"));
    }
}