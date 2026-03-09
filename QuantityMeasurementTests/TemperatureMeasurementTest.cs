using NUnit.Framework;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class TemperatureMeasurementTest
    {
        [Test]
        public void TemperatureEquality_CelsiusAndFahrenheit_ShouldBeEqual()
        {
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);
            var fahrenheit = new Quantity<TemperatureUnit>(32.0, TemperatureUnit.Fahrenheit);

            Assert.That(celsius.Equals(fahrenheit), Is.True);
        }

        [Test]
        public void TemperatureEquality_KelvinAndCelsius_ShouldBeEqual()
        {
            var kelvin = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);
            var celsius = new Quantity<TemperatureUnit>(0.0, TemperatureUnit.Celsius);

            Assert.That(kelvin.Equals(celsius), Is.True);
        }

        [Test]
        public void TemperatureConversion_CelsiusToFahrenheit_ShouldReturn212()
        {
            var celsius = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var result = celsius.ConvertTo(TemperatureUnit.Fahrenheit);

            Assert.That(result.Value, Is.EqualTo(212.0).Within(0.01));
            Assert.That(result.Unit, Is.EqualTo(TemperatureUnit.Fahrenheit));
        }

        [Test]
        public void TemperatureConversion_KelvinToCelsius_ShouldReturnZero()
        {
            var kelvin = new Quantity<TemperatureUnit>(273.15, TemperatureUnit.Kelvin);
            var result = kelvin.ConvertTo(TemperatureUnit.Celsius);

            Assert.That(result.Value, Is.EqualTo(0.0).Within(0.01));
            Assert.That(result.Unit, Is.EqualTo(TemperatureUnit.Celsius));
        }

        [Test]
        public void TemperatureArithmetic_Add_ShouldThrowNotSupportedException()
        {
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() => t1.Add(t2));
        }

        [Test]
        public void TemperatureArithmetic_Subtract_ShouldThrowNotSupportedException()
        {
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() => t1.Subtract(t2));
        }

        [Test]
        public void TemperatureArithmetic_Divide_ShouldThrowNotSupportedException()
        {
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() => t1.Divide(t2));
        }

        [Test]
        public void TemperatureServiceArithmetic_ShouldThrowNotSupportedException()
        {
            var service = new QuantityMeasurementService();
            var t1 = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var t2 = new Quantity<TemperatureUnit>(50.0, TemperatureUnit.Celsius);

            Assert.Throws<NotSupportedException>(() => service.GenericAdd(t1, t2));
            Assert.Throws<NotSupportedException>(() => service.GenericSubtract(t1, t2));
            Assert.Throws<NotSupportedException>(() => service.GenericDivide(t1, t2));
        }

        [Test]
        public void TemperatureCrossCategoryEquality_ShouldReturnFalse()
        {
            var temp = new Quantity<TemperatureUnit>(100.0, TemperatureUnit.Celsius);
            var length = new Quantity<LengthUnit>(100.0, LengthUnit.Feet);

            Assert.That(temp.Equals(length), Is.False);
        }

        [Test]
        public void OperationSupportMethods_ShouldMatchCategoryRules()
        {
            Assert.That(TemperatureUnit.Celsius.SupportsArithmeticOperation(), Is.False);
            Assert.That(LengthUnit.Feet.SupportsArithmeticOperation(), Is.True);
            Assert.That(WeightUnit.Kilogram.SupportsArithmeticOperation(), Is.True);
            Assert.That(VolumeUnit.Litre.SupportsArithmeticOperation(), Is.True);
        }

        [Test]
        public void TemperatureValidateOperationSupport_ShouldThrowForAdd()
        {
            Assert.Throws<NotSupportedException>(
                () => TemperatureUnit.Celsius.ValidateOperationSupport(ArithmeticOperation.Add));
        }
    }
}
