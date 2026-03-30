using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;

using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Cache;
using QuantityMeasurementRepository.Database;

using QuantityMeasurementConsoleApp;

class Program
{
    static void Main(string[] args)
    {
        // Step 1: Load configuration from appsettings.json
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        // Step 2: Get DB connection string
        string connectionString =
            config.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Missing ConnectionStrings:DefaultConnection in appsettings.json");

        // Step 3: Get Redis connection string
        string redisConn = config["Redis"] ?? "localhost:6379";

        // Step 4: Ask user for repository type
        Console.WriteLine("\n===== Repository Selection Menu =====");
        Console.WriteLine("1. Cache (In-memory)");
        Console.WriteLine("2. Redis");
        Console.Write("Enter choice (1 or 2): ");
        string? choice = Console.ReadLine()?.Trim();

        IQuantityMeasurementRepository repo;

        switch (choice)
        {
            case "1":
                repo = QuantityMeasurementCacheRepository.Instance;
                Console.WriteLine("[Selected: In-memory Cache Repository]");
                break;

            case "2":
                try
                {
                    // DB repository
                    var innerRepo = new QuantityMeasurementDatabaseRepository(connectionString, poolSize: 5);

                    // Redis config
                    var redisOptions = new RedisCacheOptions
                    {
                        Configuration = redisConn,
                        InstanceName = "QuantityMeasurement:"
                    };

                    // Redis cache object
                    IDistributedCache distributedCache = new RedisCache(redisOptions);

                    // Redis + DB combined repo
                    repo = new QuantityMeasurementRedisCacheRepository(innerRepo, distributedCache);

                    Console.WriteLine("[Selected: Redis Cache Repository (backed by DB)]");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Redis failed: {ex.Message}. Falling back to Cache.");
                    repo = QuantityMeasurementCacheRepository.Instance;
                }
                break;

            default:
                Console.WriteLine("Invalid choice. Defaulting to Cache.");
                repo = QuantityMeasurementCacheRepository.Instance;
                break;
        }

        // Step 5: Run Menu
        var menu = new Menu(repo);
        menu.ShowMenu();
    }
}