using NUnit.Framework;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class VolumeTest
    {
        private QuantityMeasurementService _service;

        [SetUp]
        public void Setup()
        {
            _service = new QuantityMeasurementService();
        }

        // ===== Generic Equality Tests =====
        [Test]
        public void GenericEquality_FeetToInch_ShouldBeEqual()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            Assert.That(_service.GenericAreEqual(q1, q2), Is.True);
        }

        [Test]
        public void GenericEquality_YardToFeet_ShouldBeEqual()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Yard);
            var q2 = new Quantity<LengthUnit>(3, LengthUnit.Feet);
            Assert.That(_service.GenericAreEqual(q1, q2), Is.True);
        }

        [Test]
        public void GenericEquality_CmToInch_ShouldBeEqual()
        {
            var q1 = new Quantity<LengthUnit>(2.54, LengthUnit.Cm);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Inch);
            Assert.That(_service.GenericAreEqual(q1, q2), Is.True);
        }

        [Test]
        public void GenericEquality_FeetAndYard_ShouldNotBeEqual()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Yard);
            Assert.That(_service.GenericAreEqual(q1, q2), Is.False);
        }

        // ===== Generic Conversion Tests =====
        [Test]
        public void GenericConversion_FeetToInch_ShouldReturn12()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var result = _service.GenericConvert(q, LengthUnit.Inch);
            Assert.That(result.Value, Is.EqualTo(12).Within(0.0001));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Inch));
        }

        [Test]
        public void GenericConversion_YardToCm_ShouldReturn91Point44()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Yard);
            var result = _service.GenericConvert(q, LengthUnit.Cm);
            Assert.That(result.Value, Is.EqualTo(91.44).Within(0.0001));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Cm));
        }

        [Test]
        public void GenericConversion_InchToCm_ShouldReturn2Point54()
        {
            var q = new Quantity<LengthUnit>(1, LengthUnit.Inch);
            var result = _service.GenericConvert(q, LengthUnit.Cm);
            Assert.That(result.Value, Is.EqualTo(2.54).Within(0.0001));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Cm));
        }

        // ===== Generic Addition Tests =====
        [Test]
        public void GenericAddition_FeetPlusInch_ToFeet_ShouldReturn2Point08333()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(10, LengthUnit.Inch);
            var result = _service.GenericAdd(q1, q2, LengthUnit.Feet);
            Assert.That(result.Value, Is.EqualTo(1.83333).Within(0.0001));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void GenericAddition_YardPlusFeet_ToInch_ShouldReturn144()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Yard);
            var q2 = new Quantity<LengthUnit>(3, LengthUnit.Feet);
            var result = _service.GenericAdd(q1, q2, LengthUnit.Inch);
            Assert.That(result.Value, Is.EqualTo(144).Within(0.0001));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Inch));
        }

        [Test]
        public void GenericAddition_CmPlusInch_ToCm_ShouldReturn5Point08()
        {
            var q1 = new Quantity<LengthUnit>(2.54, LengthUnit.Cm);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Inch);
            var result = _service.GenericAdd(q1, q2, LengthUnit.Cm);
            Assert.That(result.Value, Is.EqualTo(5.08).Within(0.0001));
            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Cm));
        }

        // ===== Generic Negative Tests =====
        [Test]
        public void GenericEquality_UnsupportedUnit_ShouldThrow()
        {
            var q1 = new Quantity<string>("1", "Feet"); // invalid
            var q2 = new Quantity<string>("12", "Inch");
            Assert.That(() => _service.GenericAreEqual(q1, q2), Throws.Exception);
        }

        [Test]
        public void GenericAddition_UnsupportedUnit_ShouldThrow()
        {
            var q1 = new Quantity<string>("1", "Feet"); // invalid
            var q2 = new Quantity<string>("12", "Inch");
            Assert.That(() => _service.GenericAdd(q1, q2, "Feet"), Throws.Exception);
        }

        [Test]
        public void GenericConversion_UnsupportedUnit_ShouldThrow()
        {
            var q = new Quantity<string>("1", "Feet"); // invalid
            Assert.That(() => _service.GenericConvert(q, "Inch"), Throws.Exception);
        }
    }
}