using NUnit.Framework;
using QuantityMeasurementModel.Models;
using QuantityMeasurementRepository;
using System;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class QuantityMeasurementUC5Tests
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // ===== UC5: Unit-to-Unit Conversion =====

        // Basic conversions
        [Test] public void Convert_FeetToInches() => Assert.That(service.Convert(1.0, LengthUnit.Feet, LengthUnit.Inch), Is.EqualTo(12.0).Within(1e-6));
        [Test] public void Convert_InchesToFeet() => Assert.That(service.Convert(24.0, LengthUnit.Inch, LengthUnit.Feet), Is.EqualTo(2.0).Within(1e-6));
        [Test] public void Convert_YardsToInches() => Assert.That(service.Convert(1.0, LengthUnit.Yard, LengthUnit.Inch), Is.EqualTo(36.0).Within(1e-6));
        [Test] public void Convert_InchesToYards() => Assert.That(service.Convert(72.0, LengthUnit.Inch, LengthUnit.Yard), Is.EqualTo(2.0).Within(1e-6));
        [Test] public void Convert_FeetToYard() => Assert.That(service.Convert(6.0, LengthUnit.Feet, LengthUnit.Yard), Is.EqualTo(2.0).Within(1e-6));
        [Test] public void Convert_CentimetersToInches() => Assert.That(service.Convert(2.54, LengthUnit.Cm, LengthUnit.Inch), Is.EqualTo(1.0).Within(1e-6));
[Test] 
public void Convert_InchesToCentimeters() => 
    Assert.That(service.Convert(1.0, LengthUnit.Inch, LengthUnit.Cm), Is.EqualTo(2.54).Within(1e-6));        [Test] public void Convert_YardsToFeet() => Assert.That(service.Convert(2.0, LengthUnit.Yard, LengthUnit.Feet), Is.EqualTo(6.0).Within(1e-6));
        [Test] public void Convert_CentimetersToFeet() => Assert.That(service.Convert(30.48, LengthUnit.Cm, LengthUnit.Feet), Is.EqualTo(1.0).Within(1e-6));

        // Round-trip conversions
        [Test]
        public void RoundTripConversion_FeetToInchesAndBack()
        {
            double original = 5.0;
            double converted = service.Convert(service.Convert(original, LengthUnit.Feet, LengthUnit.Inch), LengthUnit.Inch, LengthUnit.Feet);
            Assert.That(converted, Is.EqualTo(original).Within(1e-6));
        }

        // Zero and negative value conversions
        [Test] public void Convert_ZeroValue() => Assert.That(service.Convert(0.0, LengthUnit.Feet, LengthUnit.Inch), Is.EqualTo(0.0));
        [Test] public void Convert_NegativeValue() => Assert.That(service.Convert(-1.0, LengthUnit.Feet, LengthUnit.Inch), Is.EqualTo(-12.0).Within(1e-6));

        // Precision tolerance
        [Test] public void Convert_CentimetersPrecisionCheck() => Assert.That(service.Convert(1.0, LengthUnit.Cm, LengthUnit.Inch), Is.EqualTo(0.393701).Within(1e-6));

        // Invalid input handling
        [Test]
        public void Convert_InvalidUnit_Throws()
        {
            Assert.Throws<ArgumentException>(() => service.Convert(1.0, (LengthUnit)999, LengthUnit.Feet));
            Assert.Throws<ArgumentException>(() => service.Convert(1.0, LengthUnit.Feet, (LengthUnit)999));
        }

        [Test]
        public void Convert_NaNOrInfinity_Throws()
        {
            Assert.Throws<ArgumentException>(() => service.Convert(double.NaN, LengthUnit.Feet, LengthUnit.Inch));
            Assert.Throws<ArgumentException>(() => service.Convert(double.PositiveInfinity, LengthUnit.Feet, LengthUnit.Inch));
            Assert.Throws<ArgumentException>(() => service.Convert(double.NegativeInfinity, LengthUnit.Feet, LengthUnit.Inch));
        }
    }
}
