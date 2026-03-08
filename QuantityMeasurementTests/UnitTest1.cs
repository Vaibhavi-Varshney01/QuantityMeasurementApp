using NUnit.Framework;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class QuantityMeasurementTests
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // ===== FEET TEST CASES =====
        [Test]
        public void testEquality_SameValue_Feet()
        {
            Feet f1 = new Feet(5.0);
            Feet f2 = new Feet(5.0);
            Assert.That(f1.Equals(f2), Is.True);
        }

        [Test]
        public void testEquality_DifferentValue_Feet()
        {
            Feet f1 = new Feet(5.0);
            Feet f2 = new Feet(6.0);
            Assert.That(f1.Equals(f2), Is.False);
        }

        [Test]
        public void testEquality_NullComparison_Feet()
        {
            Feet f1 = new Feet(5.0);
            Feet? f2 = null;
            Assert.That(f1.Equals(f2), Is.False);
        }

        [Test]
        public void testEquality_NonNumericInput_Feet()
        {
            // Non-numeric input check, service handles validation
            Assert.Throws<System.ArgumentException>(() => service.ValidateFeetInput("abc"));
        }

        [Test]
        public void testEquality_SameReference_Feet()
        {
            Feet f1 = new Feet(5.0);
            Feet f2 = f1;
            Assert.That(f1.Equals(f2), Is.True);
        }

        // ===== INCHES TEST CASES =====
        [Test]
        public void testEquality_SameValue_Inches()
        {
            Inches i1 = new Inches(12.0);
            Inches i2 = new Inches(12.0);
            Assert.That(i1.Equals(i2), Is.True);
        }

        [Test]
        public void testEquality_DifferentValue_Inches()
        {
            Inches i1 = new Inches(12.0);
            Inches i2 = new Inches(10.0);
            Assert.That(i1.Equals(i2), Is.False);
        }

        [Test]
        public void testEquality_NullComparison_Inches()
        {
            Inches i1 = new Inches(12.0);
            Inches? i2 = null;
            Assert.That(i1.Equals(i2), Is.False);
        }

        [Test]
        public void testEquality_NonNumericInput_Inches()
        {
            // Non-numeric input check, service handles validation
            Assert.Throws<System.ArgumentException>(() => service.ValidateInchesInput("xyz"));
        }

        [Test]
        public void testEquality_SameReference_Inches()
        {
            Inches i1 = new Inches(12.0);
            Inches i2 = i1;
            Assert.That(i1.Equals(i2), Is.True);
        }
    }
}