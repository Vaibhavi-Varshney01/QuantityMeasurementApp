using Microsoft.Extensions.Configuration;
using System;

namespace QuantityMeasurementRepository.Config
{
    public class AppConfig
    {
        private static AppConfig? _instance;
        private readonly IConfiguration _config;

        private AppConfig(IConfiguration config)
        {
            _config = config;
        }

        public static AppConfig Instance => 
            _instance ??= new AppConfig(new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .Build());

        public string GetRepositoryType() => 
            _config["Database:RepositoryType"] ?? "database";

        public string GetConnectionString() => 
            _config.GetConnectionString("DefaultConnection") ?? "";

        public int GetMaxPoolSize() => 
            int.TryParse(_config["Database:MaxPoolSize"], out int size) ? size : 5;
    }
}
