using NUnit.Framework;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementTests
{
    public class LengthUnitConversionTests
    {
        // ---------- Convert To Base Unit ----------

        [Test]
        public void Convert_Inch_To_BaseUnit_Feet()
        {
            double result = LengthUnit.Inch.ConvertToBaseUnit(12);
            Assert.That(result, Is.EqualTo(1).Within(0.001));
        }

        [Test]
        public void Convert_Yard_To_BaseUnit_Feet()
        {
            double result = LengthUnit.Yard.ConvertToBaseUnit(1);
            Assert.That(result, Is.EqualTo(3).Within(0.001));
        }

        [Test]
        public void Convert_Cm_To_BaseUnit_Feet()
        {
            double result = LengthUnit.Cm.ConvertToBaseUnit(30.48);
            Assert.That(result, Is.EqualTo(1).Within(0.001));
        }

        [Test]
        public void Convert_Feet_To_BaseUnit_Feet()
        {
            double result = LengthUnit.Feet.ConvertToBaseUnit(5);
            Assert.That(result, Is.EqualTo(5).Within(0.001));
        }

        // ---------- Convert From Base Unit ----------

        [Test]
        public void Convert_BaseUnit_To_Inch()
        {
            double result = LengthUnit.Inch.ConvertFromBaseUnit(1);
            Assert.That(result, Is.EqualTo(12).Within(0.001));
        }

        [Test]
        public void Convert_BaseUnit_To_Yard()
        {
            double result = LengthUnit.Yard.ConvertFromBaseUnit(3);
            Assert.That(result, Is.EqualTo(1).Within(0.001));
        }

        [Test]
        public void Convert_BaseUnit_To_Cm()
        {
            double result = LengthUnit.Cm.ConvertFromBaseUnit(1);
            Assert.That(result, Is.EqualTo(30.48).Within(0.001));
        }

        [Test]
        public void Convert_BaseUnit_To_Feet()
        {
            double result = LengthUnit.Feet.ConvertFromBaseUnit(10);
            Assert.That(result, Is.EqualTo(10).Within(0.001));
        }

        // ---------- Edge Cases ----------

        [Test]
        public void Convert_ZeroValue_ShouldReturnZero()
        {
            double result = LengthUnit.Inch.ConvertToBaseUnit(0);
            Assert.That(result, Is.EqualTo(0).Within(0.001));
        }

        [Test]
        public void Convert_NegativeValue_ShouldReturnNegativeFeet()
        {
            double result = LengthUnit.Inch.ConvertToBaseUnit(-12);
            Assert.That(result, Is.EqualTo(-1).Within(0.001));
        }

        [Test]
        public void Convert_LargeCmValue_To_Feet()
        {
            double result = LengthUnit.Cm.ConvertToBaseUnit(3048);
            Assert.That(result, Is.EqualTo(100).Within(0.001));
        }

        [Test]
        public void Convert_LargeFeet_To_Cm()
        {
            double result = LengthUnit.Cm.ConvertFromBaseUnit(100);
            Assert.That(result, Is.EqualTo(3048).Within(0.001));
        }
    }
}