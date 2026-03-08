using NUnit.Framework;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Unit;
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
            LengthUnit u = LengthUnit.FEET;
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
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);

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
            var q = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var converted = q.ConvertTo(LengthUnit.INCHES);
            Assert.AreEqual(12.0, converted.Value);
            Assert.AreEqual(LengthUnit.INCHES, converted.Unit);
        }

        [Test]
        public void testGenericQuantity_WeightOperations_Conversion()
        {
            var q = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            var converted = q.ConvertTo(WeightUnit.GRAM);
            Assert.AreEqual(1000.0, converted.Value);
            Assert.AreEqual(WeightUnit.GRAM, converted.Unit);
        }

        // ===== Addition Tests =====
        [Test]
        public void testGenericQuantity_LengthOperations_Addition()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);
            var sum = q1.Add(q2, LengthUnit.FEET);

            Assert.AreEqual(2.0, sum.Value);
            Assert.AreEqual(LengthUnit.FEET, sum.Unit);
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
            var length = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var weight = new Quantity<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(length.Equals(weight));
        }

        // ===== Constructor Validation =====
        [Test]
        public void testGenericQuantity_ConstructorValidation_NullUnit()
        {
            Assert.Throws<ArgumentException>(() => new Quantity<LengthUnit>(1.0, null));
        }

        [Test]
        public void testGenericQuantity_ConstructorValidation_InvalidValue()
        {
            Assert.Throws<ArgumentException>(() => new Quantity<LengthUnit>(Double.NaN, LengthUnit.FEET));
            Assert.Throws<ArgumentException>(() => new Quantity<LengthUnit>(Double.PositiveInfinity, LengthUnit.FEET));
        }

        // ===== Comprehensive Conversion / Addition Checks =====
        [Test]
        public void testGenericQuantity_Conversion_AllUnitCombinations()
        {
            var feet = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var inches = feet.ConvertTo(LengthUnit.INCHES);
            var cm = inches.ConvertTo(LengthUnit.CENTIMETERS);

            Assert.AreEqual(30.48, Math.Round(cm.Value, 2));
        }

        [Test]
        public void testGenericQuantity_Addition_AllUnitCombinations()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(24.0, LengthUnit.INCHES);
            var sum = q1.Add(q2, LengthUnit.FEET);

            Assert.AreEqual(3.0, sum.Value);
        }
    }
}