using NUnit.Framework;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;
using static QuantityMeasurementApp.Model.LengthUnit;
using static QuantityMeasurementApp.Model.WeightUnit;
using static QuantityMeasurementApp.Model.VolumeUnit;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class UC12Tests
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        #region Subtraction Tests

        [Test]
        public void testSubtraction_SameUnit_FeetMinusFeet()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(5, FEET);
            var result = service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(5));
            Assert.That(result.Unit, Is.EqualTo(FEET));
        }

        [Test]
        public void testSubtraction_SameUnit_LitreMinusLitre()
        {
            var q1 = new Quantity<VolumeUnit>(10, LITRE);
            var q2 = new Quantity<VolumeUnit>(3, LITRE);
            var result = service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(7));
            Assert.That(result.Unit, Is.EqualTo(LITRE));
        }

        [Test]
        public void testSubtraction_CrossUnit_FeetMinusInches()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(6, INCHES);
            var result = service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(9.5));
            Assert.That(result.Unit, Is.EqualTo(FEET));
        }

        [Test]
        public void testSubtraction_CrossUnit_InchesMinusFeet()
        {
            var q1 = new Quantity<LengthUnit>(120, INCHES);
            var q2 = new Quantity<LengthUnit>(5, FEET);
            var result = service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(60));
            Assert.That(result.Unit, Is.EqualTo(INCHES));
        }

        [Test]
        public void testSubtraction_ExplicitTargetUnit_Feet()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(6, INCHES);
            var result = service.GenericSubtract(q1, q2, FEET);
            Assert.That(result.Value, Is.EqualTo(9.5));
            Assert.That(result.Unit, Is.EqualTo(FEET));
        }

        [Test]
        public void testSubtraction_ExplicitTargetUnit_Inches()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(6, INCHES);
            var result = service.GenericSubtract(q1, q2, INCHES);
            Assert.That(result.Value, Is.EqualTo(114));
            Assert.That(result.Unit, Is.EqualTo(INCHES));
        }

        [Test]
        public void testSubtraction_ExplicitTargetUnit_Millilitre()
        {
            var q1 = new Quantity<VolumeUnit>(5, LITRE);
            var q2 = new Quantity<VolumeUnit>(2, LITRE);
            var result = service.GenericSubtract(q1, q2, MILLILITRE);
            Assert.That(result.Value, Is.EqualTo(3000));
            Assert.That(result.Unit, Is.EqualTo(MILLILITRE));
        }

        [Test]
        public void testSubtraction_ResultingInNegative()
        {
            var q1 = new Quantity<LengthUnit>(5, FEET);
            var q2 = new Quantity<LengthUnit>(10, FEET);
            var result = service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(-5));
        }

        [Test]
        public void testSubtraction_ResultingInZero()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(120, INCHES);
            var result = service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(0));
        }

        [Test]
        public void testSubtraction_WithZeroOperand()
        {
            var q1 = new Quantity<LengthUnit>(5, FEET);
            var q2 = new Quantity<LengthUnit>(0, INCHES);
            var result = service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(5));
        }

        [Test]
        public void testSubtraction_WithNegativeValues()
        {
            var q1 = new Quantity<LengthUnit>(5, FEET);
            var q2 = new Quantity<LengthUnit>(-2, FEET);
            var result = service.GenericSubtract(q1, q2);
            Assert.That(result.Value, Is.EqualTo(7));
        }

        [Test]
        public void testSubtraction_NonCommutative()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(5, FEET);
            var r1 = service.GenericSubtract(q1, q2);
            var r2 = service.GenericSubtract(q2, q1);
            Assert.That(r1.Value, Is.EqualTo(5));
            Assert.That(r2.Value, Is.EqualTo(-5));
        }

        [Test]
        public void testSubtraction_NullOperand()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            Assert.That(() => service.GenericSubtract<LengthUnit>(q1, null!), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void testSubtraction_NullTargetUnit()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(5, FEET);
            Assert.That(() => service.GenericSubtract(q1, q2, null!), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void testSubtraction_CrossCategory()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<WeightUnit>(5, KILOGRAM);
            Assert.That(() => service.GenericSubtract<LengthUnit, WeightUnit>(q1, q2), Throws.TypeOf<ArgumentException>());
        }

        #endregion

        #region Division Tests

        [Test]
        public void testDivision_SameUnit_FeetDividedByFeet()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(2, FEET);
            var result = service.GenericDivide(q1, q2);
            Assert.That(result, Is.EqualTo(5));
        }

        [Test]
        public void testDivision_CrossUnit_FeetDividedByInches()
        {
            var q1 = new Quantity<LengthUnit>(24, INCHES);
            var q2 = new Quantity<LengthUnit>(2, FEET);
            var result = service.GenericDivide(q1, q2);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void testDivision_CrossUnit_KilogramDividedByGram()
        {
            var q1 = new Quantity<WeightUnit>(2, KILOGRAM);
            var q2 = new Quantity<WeightUnit>(2000, GRAM);
            var result = service.GenericDivide(q1, q2);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void testDivision_RatioGreaterThanOne()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(2, FEET);
            var result = service.GenericDivide(q1, q2);
            Assert.That(result, Is.GreaterThan(1));
        }

        [Test]
        public void testDivision_RatioLessThanOne()
        {
            var q1 = new Quantity<LengthUnit>(5, FEET);
            var q2 = new Quantity<LengthUnit>(10, FEET);
            var result = service.GenericDivide(q1, q2);
            Assert.That(result, Is.LessThan(1));
        }

        [Test]
        public void testDivision_RatioEqualToOne()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(10, FEET);
            var result = service.GenericDivide(q1, q2);
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void testDivision_NonCommutative()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(5, FEET);
            var r1 = service.GenericDivide(q1, q2);
            var r2 = service.GenericDivide(q2, q1);
            Assert.That(r1, Is.EqualTo(2));
            Assert.That(r2, Is.EqualTo(0.5));
        }

        [Test]
        public void testDivision_ByZero()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<LengthUnit>(0, FEET);
            Assert.That(() => service.GenericDivide(q1, q2), Throws.TypeOf<ArithmeticException>());
        }

        [Test]
        public void testDivision_NullOperand()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            Assert.That(() => service.GenericDivide<LengthUnit>(q1, null!), Throws.TypeOf<ArgumentException>());
        }

        [Test]
        public void testDivision_CrossCategory()
        {
            var q1 = new Quantity<LengthUnit>(10, FEET);
            var q2 = new Quantity<WeightUnit>(5, KILOGRAM);
            Assert.That(() => service.GenericDivide<LengthUnit, WeightUnit>(q1, q2), Throws.TypeOf<ArgumentException>());
        }

        #endregion
    }
}