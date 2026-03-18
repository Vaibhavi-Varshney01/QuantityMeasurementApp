using QuantityMeasurementApp;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Config;
using QuantityMeasurementRepository.Database;

class Program
{
    static void Main()
    {
        var config = AppConfig.Instance;
        IQuantityMeasurementRepository repo;

        string repoType = config.GetRepositoryType();
        Console.WriteLine($"[App] Repository type: {repoType}");

        if (repoType.Equals("database", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("[App] Using Database Repository (SQL Server).");
            repo = new QuantityMeasurementDatabaseRepository(
                config.GetConnectionString(),
                config.GetMaxPoolSize());
        }
        else
        {
            Console.WriteLine("[App] Using Cache Repository (in-memory).");
            repo = QuantityMeasurementCacheRepository.Instance;
        }

        IMenu menu = new Menu(repo);
        menu.ShowMenu();

        // Report all stored measurements at end
        var all = repo.GetAllMeasurements();
        Console.WriteLine($"\n[App] Total measurements stored: {all.Count}");
        foreach (var m in all)
            Console.WriteLine($"  → [{m.MeasurementType}] {m.OperationType}");

        Console.WriteLine($"[App] Pool stats: {repo.GetPoolStatistics()}");

        // Clean up
        DeleteAllMeasurements(repo);
        repo.ReleaseResources();
    }

    static void DeleteAllMeasurements(IQuantityMeasurementRepository repo)
    {
        repo.DeleteAll();
        Console.WriteLine("[App] All measurements cleared on exit.");
    }
}
