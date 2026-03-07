using NUnit.Framework;
using QuantityMeasurementApp.Service;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class QuantityMeasurementTests
    {
        private QuantityMeasurementService _service;

        [SetUp]
        public void Setup()
        {
            _service = new QuantityMeasurementService();
        }

        [Test]
        public void TestEquality_SameValue()
        {
            Feet first = new Feet(1.0);
            Feet second = new Feet(1.0);

            bool result = _service.AreFeetEqual(first, second);
            Assert.That(result, Is.True, "1.0 ft should be equal to 1.0 ft");
        }

        [Test]
        public void TestEquality_DifferentValue()
        {
            Feet first = new Feet(1.0);
            Feet second = new Feet(2.0);

            bool result = _service.AreFeetEqual(first, second);
            Assert.That(result, Is.False, "1.0 ft should not be equal to 2.0 ft");
        }

        [Test]
        public void TestEquality_NullComparison()
        {
            Feet first = new Feet(1.0);
            Feet second = null;

            bool result = _service.AreFeetEqual(first, second);
            Assert.That(result, Is.False, "Comparing with null should return false");
        }

        [Test]
        public void TestEquality_SameReference()
        {
            Feet first = new Feet(1.0);
            Feet second = first;

            bool result = _service.AreFeetEqual(first, second);
            Assert.That(result, Is.True, "Same reference should return true");
        }
    }
}