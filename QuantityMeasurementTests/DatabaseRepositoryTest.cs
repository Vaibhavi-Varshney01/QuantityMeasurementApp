using NUnit.Framework;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Config;
using QuantityMeasurementRepository.Database;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class DatabaseRepositoryTest
    {
        // points to your test-only database — never touches QuantityDB
        private const string TestConnectionString =
            "Server=localhost\\SQLEXPRESS;Database=QuantityDB_Test;" +
            "Trusted_Connection=True;TrustServerCertificate=True;";

        private QuantityMeasurementDatabaseRepository _repo = null!;

        [SetUp]
        public void Setup()
        {
            _repo = new QuantityMeasurementDatabaseRepository(
                TestConnectionString, poolSize: 3);
            _repo.DeleteAll(); // clean before every test
        }

        [TearDown]
        public void TearDown()
        {
            _repo.DeleteAll(); // clean after every test
            _repo.Dispose();   // release SQL connections
        }

        private static QuantityMeasurementEntity MakeEntity(
            string opType, string measType)
            => new QuantityMeasurementEntity(
                operand1: opType,
                operationType: opType,
                result: opType,
                measurementType: measType);

        // ── repository CRUD tests ─────────────────────────────────────────────

        [Test]
        public void testDatabaseRepository_Save_Success()
        {
            _repo.Save(MakeEntity("ADD", "Length"));
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
        }

        [Test]
        public void testDatabaseRepository_GetAllMeasurements_ReturnsAll()
        {
            _repo.Save(MakeEntity("ADD", "Length"));
            _repo.Save(MakeEntity("COMPARE", "Weight"));
            Assert.That(_repo.GetAllMeasurements().Count, Is.EqualTo(2));
        }

        [Test]
        public void testDatabaseRepository_GetMeasurementsByType_FiltersCorrectly()
        {
            _repo.Save(MakeEntity("ADD", "Length"));
            _repo.Save(MakeEntity("ADD", "Weight"));

            var result = _repo.GetMeasurementsByType("Length");

            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0].MeasurementType, Is.EqualTo("Length"));
        }

        [Test]
        public void testDatabaseRepository_GetMeasurementsByOperation_FiltersCorrectly()
        {
            _repo.Save(MakeEntity("ADD",     "Length"));
            _repo.Save(MakeEntity("COMPARE", "Weight"));

            var result = _repo.GetMeasurementsByOperation("ADD");

            Assert.That(result.Count, Is.EqualTo(1));
        }

        [Test]
        public void testDatabaseRepository_GetTotalCount_ReturnsCorrectNumber()
        {
            _repo.Save(MakeEntity("ADD", "Length"));
            _repo.Save(MakeEntity("ADD", "Length"));
            _repo.Save(MakeEntity("ADD", "Length"));

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(3));
        }

        [Test]
        public void testDatabaseRepository_DeleteAll_ClearsAllRecords()
        {
            _repo.Save(MakeEntity("ADD", "Length"));
            _repo.DeleteAll();

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        [Test]
        public void testDatabaseRepository_Save_NullEntity_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => _repo.Save(null!));
        }

        [Test]
        public void testDatabaseRepository_IsolationBetweenTests_DatabaseIsClean()
        {
            // SetUp calls DeleteAll() so every test starts with zero rows
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        [Test]
        public void testDatabaseRepository_PersistsMultipleOperations()
        {
            _repo.Save(MakeEntity("ADD",     "Length"));
            _repo.Save(MakeEntity("COMPARE", "Weight"));
            _repo.Save(MakeEntity("CONVERT", "Volume"));

            Assert.That(_repo.GetTotalCount(),                          Is.EqualTo(3));
            Assert.That(_repo.GetMeasurementsByType("Length").Count,    Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByType("Weight").Count,    Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByType("Volume").Count,    Is.EqualTo(1));
        }

        [Test]
        public void testDatabaseRepository_PoolStatistics_ReturnsInfo()
        {
            string stats = _repo.GetPoolStatistics();
            Assert.That(stats, Is.Not.Null.And.Not.Empty);
        }

        // ── DatabaseException tests ───────────────────────────────────────────

        [Test]
        public void testDatabaseException_HasCorrectOperation()
        {
            var ex = new DatabaseException("Test error", "SAVE");
            Assert.That(ex.Operation, Is.EqualTo("SAVE"));
            Assert.That(ex.ToString(), Does.Contain("SAVE"));
        }

        [Test]
        public void testDatabaseException_WithInnerException()
        {
            var inner = new Exception("inner");
            var ex = new DatabaseException("outer", inner, "GET_ALL");
            Assert.That(ex.InnerException, Is.EqualTo(inner));
            Assert.That(ex.Operation, Is.EqualTo("GET_ALL"));
        }

        // ── AppConfig tests ───────────────────────────────────────────────────

        [Test]
        public void testAppConfig_RepositoryType_IsValidValue()
        {
            string type = AppConfig.Instance.GetRepositoryType();
            Assert.That(type == "cache" || type == "database", Is.True);
        }

        [Test]
        public void testAppConfig_MaxPoolSize_IsPositive()
        {
            Assert.That(AppConfig.Instance.GetMaxPoolSize(), Is.GreaterThan(0));
        }

        [Test]
        public void testAppConfig_ConnectionString_IsNotEmpty()
        {
            Assert.That(AppConfig.Instance.GetConnectionString(), Is.Not.Empty);
        }
    }
}
