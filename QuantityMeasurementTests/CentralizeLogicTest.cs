using NUnit.Framework;
using QuantityMeasurementApp.Model;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityTestsUC13
    {
        // Example enum for testing
        private enum LengthUnit { INCHES, FEET }
        private enum WeightUnit { GRAM, KILOGRAM }

        [Test]
        public void testRefactoring_Add_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.INCHES);

            var result = q1.Add(q2);
            Assert.AreEqual(2.0, result.Value);
            Assert.AreEqual(LengthUnit.FEET, result.Unit);
        }

        [Test]
        public void testRefactoring_Subtract_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.INCHES);

            var result = q1.Subtract(q2);
            Assert.AreEqual(9.5, result.Value);
        }

        [Test]
        public void testRefactoring_Divide_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.FEET);

            var result = q1.Divide(q2);
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void testValidation_NullOperand_Throws()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.FEET);
            Quantity<LengthUnit> q2 = null;

            Assert.Throws<ArgumentNullException>(() => q1.Add(q2));
            Assert.Throws<ArgumentNullException>(() => q1.Subtract(q2));
            Assert.Throws<ArgumentNullException>(() => q1.Divide(q2));
        }

        [Test]
        public void testValidation_CrossCategory_Throws()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.FEET);
            var q2 = new Quantity<WeightUnit>(5, WeightUnit.GRAM);

            Assert.Throws<ArgumentException>(() => q1.Add(q2 as dynamic));
            Assert.Throws<ArgumentException>(() => q1.Subtract(q2 as dynamic));
        }

        [Test]
        public void testArithmetic_DivideByZero_Throws()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(0, LengthUnit.FEET);

            Assert.Throws<DivideByZeroException>(() => q1.Divide(q2));
        }

        [Test]
        public void testRounding_AddSubtract()
        {
            var q1 = new Quantity<LengthUnit>(1.234, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(0.123, LengthUnit.FEET);

            Assert.AreEqual(1.36, q1.Add(q2).Value);
            Assert.AreEqual(1.11, q1.Subtract(q2).Value);
        }

        [Test]
        public void testDivide_NoRounding()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.FEET);
            var q2 = new Quantity<LengthUnit>(4.0, LengthUnit.FEET);

            Assert.AreEqual(2.5, q1.Divide(q2));
        }
    }
}