using NUnit.Framework;
using QuantityMeasurementModel.Models;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityTests
    {
        // ===== IMeasurable Interface Tests =====
        [Test]
        public void testIMeasurableInterface_LengthUnitImplementation()
        {
            LengthUnit u = LengthUnit.Feet;
            Assert.AreEqual(1.0, u.ConvertToBaseUnit(1.0)); // Base unit is FEET
            Assert.AreEqual(12.0, u.ConvertFromBaseUnit(12.0));
            Assert.AreEqual("FEET", u.GetUnitName());
        }

        [Test]
        public void testIMeasurableInterface_WeightUnitImplementation()
        {
            WeightUnit u = WeightUnit.KILOGRAM;
            Assert.AreEqual(1000.0, u.ConvertToBaseUnit(1.0)); // Base unit GRAM
            Assert.AreEqual(1.0, u.ConvertFromBaseUnit(1000.0));
            Assert.AreEqual("KILOGRAM", u.GetUnitName());
        }

        // ===== Quantity<LengthUnit> Equality =====
        [Test]
        public void testGenericQuantity_LengthOperations_Equality()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            Assert.IsTrue(q1.Equals(q2));
        }

        // ===== Quantity<WeightUnit> Equality =====
        [Test]
        public void testGenericQuantity_WeightOperations_Equality()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);

            Assert.IsTrue(q1.Equals(q2));
        }

        // ===== Conversion Tests =====
        [Test]
        public void testGenericQuantity_LengthOperations_Conversion()
        {
            var q = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var converted = q.ConvertTo(LengthUnit.Inch);
            Assert.AreEqual(12.0, converted);
        }

        [Test]
        public void testGenericQuantity_WeightOperations_Conversion()
        {
            var q = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var converted = q.ConvertTo(WeightUnit.GRAM);
            Assert.AreEqual(1000.0, converted);
        }

        // ===== Addition Tests =====
        [Test]
        public void testGenericQuantity_LengthOperations_Addition()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
            var sum = q1.Add(q2, LengthUnit.Feet);

            Assert.AreEqual(2.0, sum.Value);
            Assert.AreEqual(LengthUnit.Feet, sum.Unit);
        }

        [Test]
        public void testGenericQuantity_WeightOperations_Addition()
        {
            var q1 = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var q2 = new Quantity<WeightUnit>(1000.0, WeightUnit.GRAM);
            var sum = q1.Add(q2, WeightUnit.KILOGRAM);

            Assert.AreEqual(2.0, sum.Value);
            Assert.AreEqual(WeightUnit.KILOGRAM, sum.Unit);
        }

        // ===== Cross-Category Safety =====
        [Test]
        public void testCrossCategoryPrevention_LengthVsWeight()
        {
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var weight = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(length.Equals(weight));
        }

        // ===== Constructor Validation =====
        [Test]
        public void testGenericQuantity_ConstructorValidation_NullUnit()
        {
            Assert.Throws<ArgumentException>(() => new Quantity<LengthUnit>(1.0, (LengthUnit)999));
        }

        [Test]
        public void testGenericQuantity_ConstructorValidation_InvalidValue()
        {
            Assert.Throws<ArgumentException>(() => new Quantity<LengthUnit>(Double.NaN, LengthUnit.Feet));
            Assert.Throws<ArgumentException>(() => new Quantity<LengthUnit>(Double.PositiveInfinity, LengthUnit.Feet));
        }

        // ===== Comprehensive Conversion / Addition Checks =====
        [Test]
        public void testGenericQuantity_Conversion_AllUnitCombinations()
        {
            var feet = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var inches = feet.ConvertTo(LengthUnit.Inch);
            var cm = new Quantity<LengthUnit>(inches, LengthUnit.Inch).ConvertTo(LengthUnit.Cm);

            Assert.AreEqual(30.48, Math.Round(cm, 2));
        }

        [Test]
        public void testGenericQuantity_Addition_AllUnitCombinations()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(24.0, LengthUnit.Inch);
            var sum = q1.Add(q2, LengthUnit.Feet);

            Assert.AreEqual(3.0, sum.Value);
        }
    }
}
