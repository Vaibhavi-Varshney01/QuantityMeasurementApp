using NUnit.Framework;
using QuantityMeasurementModel.Models;
using QuantityMeasurementRepository;
using System;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class QuantityMeasurementUC6Tests
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // ===== UC6: Addition Tests =====

        [Test]
        public void Addition_SameUnit_FeetPlusFeet()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);
            var result = service.Add(q1, q2);
            Assert.That(result.Value, Is.EqualTo(3.0).Within(1e-6));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void Addition_SameUnit_InchPlusInch()
        {
            var q1 = new Quantity<LengthUnit>(6.0, LengthUnit.Inch);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.Inch);
            var result = service.Add(q1, q2);
            Assert.That(result.Value, Is.EqualTo(12.0).Within(1e-6));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Inch));
        }

        [Test]
        public void Addition_CrossUnit_FeetPlusInches()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
            var result = service.Add(q1, q2);
            Assert.That(result.Value, Is.EqualTo(2.0).Within(1e-6));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void Addition_CrossUnit_InchPlusFeet()
        {
            var q1 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
            var q2 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var result = service.Add(q1, q2);
            Assert.That(result.Value, Is.EqualTo(24.0).Within(1e-6));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Inch));
        }

        [Test]
        public void Addition_CrossUnit_YardPlusFeet()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Yard);
            var q2 = new Quantity<LengthUnit>(3.0, LengthUnit.Feet);
            var result = service.Add(q1, q2);
            Assert.That(result.Value, Is.EqualTo(2.0).Within(1e-6));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Yard));
        }

        [Test]
        public void Addition_CrossUnit_CentimeterPlusInch()
        {
            var q1 = new Quantity<LengthUnit>(2.54, LengthUnit.Cm);
            var q2 = new Quantity<LengthUnit>(1.0, LengthUnit.Inch);
            var result = service.Add(q1, q2);
            Assert.That(result.Value, Is.EqualTo(5.08).Within(1e-2)); // allow small epsilon for float
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Cm));
        }

        [Test]
        public void Addition_Commutativity()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            var result1 = service.Add(q1, q2);
            var result2 = service.Add(q2, q1);

            // Convert result2 to result1's unit for comparison
            var result2InFeet = service.Convert(result2.Value, result2.Unit, result1.Unit);

            Assert.That(result1.Value, Is.EqualTo(result2InFeet).Within(1e-6));
        }

        [Test]
        public void Addition_WithZero()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0.0, LengthUnit.Inch);
            var result = service.Add(q1, q2);
            Assert.That(result.Value, Is.EqualTo(5.0).Within(1e-6));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void Addition_NegativeValues()
        {
            var q1 = new Quantity<LengthUnit>(5.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(-2.0, LengthUnit.Feet);
            var result = service.Add(q1, q2);
            Assert.That(result.Value, Is.EqualTo(3.0).Within(1e-6));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void Addition_NullOperand_Throws()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            Quantity<LengthUnit> q2 = null;

            Assert.Throws<ArgumentException>(() => service.Add(q1, q2));
        }
    }
}
