using NUnit.Framework;
using QuantityMeasurementRepository;
using QuantityMeasurementRepository.Config;
using QuantityMeasurementRepository.Database;
using QuantityMeasurementModel.Models;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementControllerLayer;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class IntegrationTest
    {
        private const string TestConnectionString =
            "Server=localhost\\SQLEXPRESS;Database=QuantityDB_Test;" +
            "Trusted_Connection=True;TrustServerCertificate=True;";

        private QuantityMeasurementDatabaseRepository _repo = null!;
        private QuantityMeasurementService            _service = null!;
        private QuantityMeasurementController         _controller = null!;

        [SetUp]
        public void Setup()
        {
            _repo       = new QuantityMeasurementDatabaseRepository(TestConnectionString, poolSize: 3);
            _service    = new QuantityMeasurementService();
            _controller = new QuantityMeasurementController(_service);
            _repo.DeleteAll(); // clean slate before every test
        }

        [TearDown]
        public void TearDown()
        {
            _repo.DeleteAll();
            _repo.Dispose();
        }

        private static QuantityMeasurementEntity MakeEntity(string opType, string measType)
            => new QuantityMeasurementEntity(
                operand1: opType,
                operationType: opType,
                result: opType,
                measurementType: measType);

        [Test]
        public void testIntegration_Controller_EqualityDemo_ReturnsTrue()
        {
            Assert.That(_controller.PerformEqualityDemo(), Is.True);
        }

        [Test]
        public void testIntegration_Controller_ConversionDemo_Returns12Inches()
        {
            var result = _controller.PerformConversionDemo();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(12));
            Assert.That(result.Unit,  Is.EqualTo(LengthUnit.Inch));
        }

        [Test]
        public void testIntegration_Controller_AdditionDemo_Returns2Feet()
        {
            var result = _controller.PerformAdditionDemo();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Value, Is.EqualTo(2));
            Assert.That(result.Unit,  Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void testIntegration_LengthAddition_SavedToDB()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = _service.Add(q1, q2, LengthUnit.Feet);

            // verify calculation is correct
            Assert.That(result.Value, Is.EqualTo(2));

            // save to DB and verify it persisted
            _repo.Save(new QuantityMeasurementEntity(
                "1 Feet", "12 Inch", "ADD", result.Value.ToString(), "Length"));

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByOperation("ADD").Count, Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByType("Length").Count,   Is.EqualTo(1));
        }

        [Test]
        public void testIntegration_LengthSubtraction_SavedToDB()
        {
            var q1 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var result = _service.Subtract(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(3));

            _repo.Save(new QuantityMeasurementEntity(
                "5 Feet", "2 Feet", "SUBTRACT", result.Value.ToString(), "Length"));

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByOperation("SUBTRACT").Count, Is.EqualTo(1));
        }

        [Test]
        public void testIntegration_LengthDivision_SavedToDB()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(5,  LengthUnit.Feet);

            double result = _service.Divide(q1, q2);

            Assert.That(result, Is.EqualTo(2));

            _repo.Save(new QuantityMeasurementEntity(
                "10 Feet", "5 Feet", "DIVIDE", result.ToString(), "Length"));

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByOperation("DIVIDE").Count, Is.EqualTo(1));
        }

        [Test]
        public void testIntegration_LengthConversion_SavedToDB()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = _service.GenericConvert(q, LengthUnit.Inch);

            Assert.That(result.Value, Is.EqualTo(12));

            _repo.Save(new QuantityMeasurementEntity(
                operand1: "1 Feet",
                operationType: "CONVERT",
                result: "12 Inch",
                measurementType: "Length"));

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByOperation("CONVERT").Count, Is.EqualTo(1));
        }

        [Test]
        public void testIntegration_LengthComparison_SavedToDB()
        {
            var q1 = new Quantity<LengthUnit>(1,  LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            bool equal = _service.GenericAreEqual<LengthUnit>(q1, q2);

            Assert.That(equal, Is.True);

            _repo.Save(new QuantityMeasurementEntity(
                "1 Feet", "12 Inch", "COMPARE", equal.ToString(), "Length"));

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByOperation("COMPARE").Count, Is.EqualTo(1));
        }

        // ══════════════════════════════════════════════════════
        // Weight operations → DB
        // ══════════════════════════════════════════════════════

        [Test]
        public void testIntegration_WeightAddition_SavedToDB()
        {
            var q1 = new Quantity<WeightUnit>(500, WeightUnit.Gram);
            var q2 = new Quantity<WeightUnit>(500, WeightUnit.Gram);

            var result = _service.GenericAdd(q1, q2);

            Assert.That(result.Value, Is.EqualTo(1000));

            _repo.Save(new QuantityMeasurementEntity(
                "500 Gram", "500 Gram", "ADD", result.Value.ToString(), "Weight"));

            Assert.That(_repo.GetMeasurementsByType("Weight").Count, Is.EqualTo(1));
        }

        [Test]
        public void testIntegration_WeightConversion_SavedToDB()
        {
            var q = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);

            var result = _service.GenericConvert(q, WeightUnit.Gram);

            Assert.That(result.Value, Is.EqualTo(1000));

            _repo.Save(new QuantityMeasurementEntity(
                operand1: "1 Kilogram",
                operationType: "CONVERT",
                result: "1000 Gram",
                measurementType: "Weight"));

            Assert.That(_repo.GetMeasurementsByType("Weight").Count, Is.EqualTo(1));
        }

        // ══════════════════════════════════════════════════════
        // Temperature operations → DB
        // ══════════════════════════════════════════════════════

        [Test]
        public void testIntegration_TemperatureConversion_SavedToDB()
        {
            var q = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);

            var result = _service.GenericConvert(q, TemperatureUnit.Fahrenheit);

            Assert.That(result.Value, Is.EqualTo(212).Within(0.01));

            _repo.Save(new QuantityMeasurementEntity(
                operand1: "100 Celsius",
                operationType: "CONVERT",
                result: "212 Fahrenheit",
                measurementType: "Temperature"));

            Assert.That(_repo.GetMeasurementsByType("Temperature").Count, Is.EqualTo(1));
        }

        [Test]
        public void testIntegration_TemperatureAddition_NotSaved_ThrowsException()
        {
            var q1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var q2 = new Quantity<TemperatureUnit>(50,  TemperatureUnit.Celsius);

            // operation fails — error entity saved instead
            Assert.Throws<NotSupportedException>(() => _service.Add(q1, q2));

            // save the error to DB
            _repo.Save(new QuantityMeasurementEntity(
                "Temperature does not support add operation."));

            Assert.That(_repo.GetTotalCount(), Is.EqualTo(1));
            // error constructor sets OperationType = "ERROR"
            Assert.That(_repo.GetMeasurementsByOperation("ERROR").Count, Is.EqualTo(1));
        }

        // ══════════════════════════════════════════════════════
        // Multiple operations accumulate in DB
        // ══════════════════════════════════════════════════════

        [Test]
        public void testIntegration_MultipleOperations_AllSavedToDB()
        {
            _repo.Save(MakeEntity("ADD",     "Length"));
            _repo.Save(MakeEntity("SUBTRACT","Length"));
            _repo.Save(MakeEntity("COMPARE", "Weight"));
            _repo.Save(MakeEntity("CONVERT", "Temperature"));

            Assert.That(_repo.GetTotalCount(),                              Is.EqualTo(4));
            Assert.That(_repo.GetMeasurementsByType("Length").Count,        Is.EqualTo(2));
            Assert.That(_repo.GetMeasurementsByType("Weight").Count,        Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByType("Temperature").Count,   Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByOperation("ADD").Count,      Is.EqualTo(1));
            Assert.That(_repo.GetMeasurementsByOperation("SUBTRACT").Count, Is.EqualTo(1));
        }

        // ══════════════════════════════════════════════════════
        // DB isolation between tests
        // ══════════════════════════════════════════════════════

        [Test]
        public void testIntegration_IsolationBetweenTests_DBIsClean()
        {
            // SetUp calls DeleteAll() so every test starts with zero rows
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        [Test]
        public void testIntegration_DeleteAll_ResetsDB()
        {
            _repo.Save(MakeEntity("ADD", "Length"));
            _repo.Save(MakeEntity("ADD", "Length"));
            _repo.DeleteAll();
            Assert.That(_repo.GetTotalCount(), Is.EqualTo(0));
        }

        // ══════════════════════════════════════════════════════
        // AppConfig
        // ══════════════════════════════════════════════════════

        [Test]
        public void testIntegration_AppConfig_LoadedCorrectly()
        {
            var config = AppConfig.Instance;
            Assert.That(config.GetRepositoryType(),   Is.Not.Null);
            Assert.That(config.GetConnectionString(), Is.Not.Null);
            Assert.That(config.GetMaxPoolSize(),      Is.GreaterThan(0));
        }

        [Test]
        public void testIntegration_AppConfig_RepositoryType_IsValidValue()
        {
            string type = AppConfig.Instance.GetRepositoryType();
            Assert.That(type == "cache" || type == "database", Is.True);
        }

        // ══════════════════════════════════════════════════════
        // Pool statistics
        // ══════════════════════════════════════════════════════

        [Test]
        public void testIntegration_PoolStatistics_ReturnsInfo()
        {
            string stats = _repo.GetPoolStatistics();
            Assert.That(stats, Is.Not.Null.And.Not.Empty);
        }

        [Test]
        public void testIntegration_ReleaseResources_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => _repo.ReleaseResources());
        }
    }
}
