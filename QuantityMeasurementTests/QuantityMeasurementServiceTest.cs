using NUnit.Framework;
using QuantityMeasurementRepository;
using QuantityMeasurementModel.Models;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class QuantityMeasurementServiceTest
    {
        private QuantityMeasurementService _service = null!;

        [SetUp]
        public void Setup()
        {
            _service = new QuantityMeasurementService();
        }

        // ══════════════════════════════════════════════
        // AreEqual — Length
        // ══════════════════════════════════════════════

        [Test]
        public void testService_AreEqual_FeetAndInches_Equal()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            Assert.That(_service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void testService_AreEqual_SameFeet_Equal()
        {
            var q1 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            Assert.That(_service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void testService_AreEqual_DifferentValues_NotEqual()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Inch);
            Assert.That(_service.AreEqual(q1, q2), Is.False);
        }

        [Test]
        public void testService_AreEqual_NullFirst_ReturnsFalse()
        {
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            Assert.That(_service.AreEqual(null!, q2), Is.False);
        }

        [Test]
        public void testService_AreEqual_NullSecond_ReturnsFalse()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            Assert.That(_service.AreEqual(q1, null!), Is.False);
        }

        // ══════════════════════════════════════════════
        // AreFeetEqual / AreInchesEqual
        // ══════════════════════════════════════════════

        [Test]
        public void testService_AreFeetEqual_SameValue_True()
        {
            Assert.That(_service.AreFeetEqual(new Feet(5), new Feet(5)), Is.True);
        }

        [Test]
        public void testService_AreFeetEqual_DifferentValue_False()
        {
            Assert.That(_service.AreFeetEqual(new Feet(5), new Feet(6)), Is.False);
        }

        [Test]
        public void testService_AreFeetEqual_NullSecond_False()
        {
            Assert.That(_service.AreFeetEqual(new Feet(5), null!), Is.False);
        }

        [Test]
        public void testService_AreInchesEqual_SameValue_True()
        {
            Assert.That(_service.AreInchesEqual(new Inches(12), new Inches(12)), Is.True);
        }

        [Test]
        public void testService_AreInchesEqual_DifferentValue_False()
        {
            Assert.That(_service.AreInchesEqual(new Inches(12), new Inches(10)), Is.False);
        }

        [Test]
        public void testService_AreInchesEqual_NullSecond_False()
        {
            Assert.That(_service.AreInchesEqual(new Inches(12), null!), Is.False);
        }

        // ══════════════════════════════════════════════
        // Add — Length
        // ══════════════════════════════════════════════

        [Test]
        public void testService_Add_FeetAndInches_ReturnsTwoFeet()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            var result = _service.Add(q1, q2, LengthUnit.Feet);
            Assert.That(result.Value, Is.EqualTo(2));
            Assert.That(result.Unit,  Is.EqualTo(LengthUnit.Feet));
        }

        [Test]
        public void testService_Add_SameFeet_ReturnsSum()
        {
            var q1 = new Quantity<LengthUnit>(3, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var result = _service.Add(q1, q2, LengthUnit.Feet);
            Assert.That(result.Value, Is.EqualTo(5));
        }

        [Test]
        public void testService_Add_FeetAndInches_TargetInch()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            var result = _service.Add(q1, q2, LengthUnit.Inch);
            Assert.That(result.Value, Is.EqualTo(24));
        }

        [Test]
        public void testService_Add_NullFirst_ThrowsException()
        {
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            Assert.Throws<ArgumentException>(() =>
                _service.Add(null!, q2, LengthUnit.Feet));
        }

        [Test]
        public void testService_Add_NullSecond_ThrowsException()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            Assert.Throws<ArgumentException>(() =>
                _service.Add(q1, null!, LengthUnit.Feet));
        }

        // ══════════════════════════════════════════════
        // Add — Temperature (unsupported)
        // ══════════════════════════════════════════════

        [Test]
        public void testService_Add_Temperature_ThrowsNotSupported()
        {
            var q1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var q2 = new Quantity<TemperatureUnit>(50,  TemperatureUnit.Celsius);
            Assert.Throws<NotSupportedException>(() => _service.Add(q1, q2));
        }

        // ══════════════════════════════════════════════
        // Subtract
        // ══════════════════════════════════════════════

        [Test]
        public void testService_Subtract_Length_Success()
        {
            var q1 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var result = _service.Subtract(q1, q2, LengthUnit.Feet);
            Assert.That(result.Value, Is.EqualTo(3));
        }

        [Test]
        public void testService_Subtract_Length_WithTargetUnit()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            var result = _service.Subtract(q1, q2, LengthUnit.Feet);
            Assert.That(result.Value, Is.EqualTo(1));
        }

        [Test]
        public void testService_Subtract_Weight_Success()
        {
            var q1 = new Quantity<WeightUnit>(500, WeightUnit.Gram);
            var q2 = new Quantity<WeightUnit>(200, WeightUnit.Gram);
            var result = _service.Subtract(q1, q2, WeightUnit.Gram);
            Assert.That(result.Value, Is.EqualTo(300));
        }

        [Test]
        public void testService_Subtract_Temperature_ThrowsNotSupported()
        {
            var q1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var q2 = new Quantity<TemperatureUnit>(50,  TemperatureUnit.Celsius);
            Assert.Throws<NotSupportedException>(() =>
                _service.Subtract(q1, q2));
        }

        [Test]
        public void testService_Subtract_NullFirst_ThrowsException()
        {
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            Assert.Throws<ArgumentException>(() =>
                _service.Subtract<LengthUnit>(null!, q2));
        }

        // ══════════════════════════════════════════════
        // Divide
        // ══════════════════════════════════════════════

        [Test]
        public void testService_Divide_Length_Success()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(5,  LengthUnit.Feet);
            Assert.That(_service.Divide(q1, q2), Is.EqualTo(2));
        }

        [Test]
        public void testService_Divide_Weight_Success()
        {
            var q1 = new Quantity<WeightUnit>(1000, WeightUnit.Gram);
            var q2 = new Quantity<WeightUnit>(500,  WeightUnit.Gram);
            Assert.That(_service.Divide(q1, q2), Is.EqualTo(2));
        }

        [Test]
        public void testService_Divide_ByZero_ThrowsArithmeticException()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0,  LengthUnit.Feet);
            Assert.Throws<ArithmeticException>(() => _service.Divide(q1, q2));
        }

        [Test]
        public void testService_Divide_Temperature_ThrowsNotSupported()
        {
            var q1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var q2 = new Quantity<TemperatureUnit>(50,  TemperatureUnit.Celsius);
            Assert.Throws<NotSupportedException>(() =>
                _service.GenericDivide(q1, q2));
        }

        [Test]
        public void testService_Divide_NullFirst_ThrowsException()
        {
            var q2 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            Assert.Throws<ArgumentException>(() =>
                _service.Divide<LengthUnit>(null!, q2));
        }

        // ══════════════════════════════════════════════
        // GenericConvert
        // ══════════════════════════════════════════════

        [Test]
        public void testService_Convert_FeetToInch_Returns12()
        {
            var result = _service.GenericConvert(
                new Quantity<LengthUnit>(1, LengthUnit.Feet), LengthUnit.Inch);
            Assert.That(result.Value, Is.EqualTo(12));
        }

        [Test]
        public void testService_Convert_InchToFeet_Returns1()
        {
            var result = _service.GenericConvert(
                new Quantity<LengthUnit>(12, LengthUnit.Inch), LengthUnit.Feet);
            Assert.That(result.Value, Is.EqualTo(1));
        }

        [Test]
        public void testService_Convert_KgToGram_Returns1000()
        {
            var result = _service.GenericConvert(
                new Quantity<WeightUnit>(1, WeightUnit.Kilogram), WeightUnit.Gram);
            Assert.That(result.Value, Is.EqualTo(1000));
        }

        [Test]
        public void testService_Convert_GramToKg_Returns1()
        {
            var result = _service.GenericConvert(
                new Quantity<WeightUnit>(1000, WeightUnit.Gram), WeightUnit.Kilogram);
            Assert.That(result.Value, Is.EqualTo(1));
        }

        [Test]
        public void testService_Convert_CelsiusToFahrenheit_Returns212()
        {
            var result = _service.GenericConvert(
                new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius),
                TemperatureUnit.Fahrenheit);
            Assert.That(result.Value, Is.EqualTo(212).Within(0.01));
        }

        [Test]
        public void testService_Convert_FahrenheitToCelsius_Returns100()
        {
            var result = _service.GenericConvert(
                new Quantity<TemperatureUnit>(212, TemperatureUnit.Fahrenheit),
                TemperatureUnit.Celsius);
            Assert.That(result.Value, Is.EqualTo(100).Within(0.01));
        }

        [Test]
        public void testService_Convert_CelsiusToKelvin_Returns373()
        {
            var result = _service.GenericConvert(
                new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius),
                TemperatureUnit.Kelvin);
            Assert.That(result.Value, Is.EqualTo(373.15).Within(0.01));
        }

        [Test]
        public void testService_Convert_NullQuantity_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                _service.GenericConvert<LengthUnit>(null!, LengthUnit.Inch));
        }

        // ══════════════════════════════════════════════
        // GenericAreEqual
        // ══════════════════════════════════════════════

        [Test]
        public void testService_GenericAreEqual_Length_Equal()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            Assert.That(_service.GenericAreEqual<LengthUnit>(q1, q2), Is.True);
        }

        [Test]
        public void testService_GenericAreEqual_Weight_Equal()
        {
            var q1 = new Quantity<WeightUnit>(1,    WeightUnit.Kilogram);
            var q2 = new Quantity<WeightUnit>(1000, WeightUnit.Gram);
            Assert.That(_service.GenericAreEqual<WeightUnit>(q1, q2), Is.True);
        }

        [Test]
        public void testService_GenericAreEqual_Temperature_Equal()
        {
            var q1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var q2 = new Quantity<TemperatureUnit>(212, TemperatureUnit.Fahrenheit);
            Assert.That(_service.GenericAreEqual<TemperatureUnit>(q1, q2), Is.True);
        }

        [Test]
        public void testService_GenericAreEqual_CrossCategory_ThrowsException()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<WeightUnit>(1, WeightUnit.Kilogram);
            Assert.Throws<ArgumentException>(() =>
                _service.GenericAreEqual<LengthUnit, WeightUnit>(q1, q2));
        }

        // ══════════════════════════════════════════════
        // GenericAdd
        // ══════════════════════════════════════════════

        [Test]
        public void testService_GenericAdd_Length_WithTargetUnit()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            var result = _service.GenericAdd(q1, q2, LengthUnit.Feet);
            Assert.That(result.Value, Is.EqualTo(2));
        }

        [Test]
        public void testService_GenericAdd_Weight_Success()
        {
            var q1 = new Quantity<WeightUnit>(500, WeightUnit.Gram);
            var q2 = new Quantity<WeightUnit>(500, WeightUnit.Gram);
            var result = _service.GenericAdd(q1, q2);
            Assert.That(result.Value, Is.EqualTo(1000));
        }

        [Test]
        public void testService_GenericAdd_Volume_Success()
        {
            var q1 = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var q2 = new Quantity<VolumeUnit>(1, VolumeUnit.Litre);
            var result = _service.GenericAdd(q1, q2);
            Assert.That(result.Value, Is.EqualTo(2));
        }

        [Test]
        public void testService_GenericAdd_Temperature_ThrowsNotSupported()
        {
            var q1 = new Quantity<TemperatureUnit>(100, TemperatureUnit.Celsius);
            var q2 = new Quantity<TemperatureUnit>(50,  TemperatureUnit.Celsius);
            Assert.Throws<NotSupportedException>(() =>
                _service.GenericAdd(q1, q2));
        }

        // ══════════════════════════════════════════════
        // GenericSubtract
        // ══════════════════════════════════════════════

        [Test]
        public void testService_GenericSubtract_Length_Success()
        {
            var q1 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var result = _service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(3));
        }

        [Test]
        public void testService_GenericSubtract_WithTargetUnit_Success()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            var result = _service.GenericSubtract(q1, q2, LengthUnit.Feet);
            Assert.That(result.Value, Is.EqualTo(1));
        }

        [Test]
        public void testService_GenericSubtract_Weight_Success()
        {
            var q1 = new Quantity<WeightUnit>(1000, WeightUnit.Gram);
            var q2 = new Quantity<WeightUnit>(500,  WeightUnit.Gram);
            var result = _service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(500));
        }

        // ══════════════════════════════════════════════
        // GenericDivide
        // ══════════════════════════════════════════════

        [Test]
        public void testService_GenericDivide_Length_Success()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(5,  LengthUnit.Feet);
            Assert.That(_service.GenericDivide(q1, q2), Is.EqualTo(2));
        }

        [Test]
        public void testService_GenericDivide_Weight_Success()
        {
            var q1 = new Quantity<WeightUnit>(1000, WeightUnit.Gram);
            var q2 = new Quantity<WeightUnit>(500,  WeightUnit.Gram);
            Assert.That(_service.GenericDivide(q1, q2), Is.EqualTo(2));
        }

        [Test]
        public void testService_GenericDivide_ByZero_ThrowsArithmeticException()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0,  LengthUnit.Feet);
            Assert.Throws<ArithmeticException>(() =>
                _service.GenericDivide(q1, q2));
        }
    }
}
