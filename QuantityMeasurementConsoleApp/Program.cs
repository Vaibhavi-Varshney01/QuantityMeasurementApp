using QuantityMeasurementApp;
using Microsoft.Extensions.Configuration;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Database;

IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .Build();

string connectionString =
    config.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Missing ConnectionStrings:DefaultConnection in QuantityMeasurementConsoleApp/appsettings.json");

using var repository =
    new QuantityMeasurementDatabaseRepository(connectionString, poolSize: 5);

var menu = new Menu(repository);
menu.ShowMenu();
