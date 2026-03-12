using NUnit.Framework;
using QuantityMeasurementModel.Models;
using System;

namespace QuantityMeasurementApp.Tests
{
    [TestFixture]
    public class QuantityTestsUC13
    {
        [Test]
        public void testRefactoring_Add_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

            var result = q1.Add(q2);
            Assert.AreEqual(2.0, result.Value);
            Assert.AreEqual(LengthUnit.Feet, result.Unit);
        }

        [Test]
        public void testRefactoring_Subtract_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(6.0, LengthUnit.Inch);

            var result = q1.Subtract(q2);
            Assert.AreEqual(9.5, result.Value);
        }

        [Test]
        public void testRefactoring_Divide_DelegatesViaHelper()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);

            var result = q1.Divide(q2);
            Assert.AreEqual(5.0, result);
        }

        [Test]
        public void testValidation_NullOperand_Throws()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            Quantity<LengthUnit> q2 = null;

            Assert.Throws<ArgumentNullException>(() => q1.Add(q2));
            Assert.Throws<ArgumentNullException>(() => q1.Subtract(q2));
            Assert.Throws<ArgumentNullException>(() => q1.Divide(q2));
        }

        [Test]
        public void testValidation_CrossCategory_Throws()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<WeightUnit>(5, WeightUnit.GRAM);

            Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => q1.Add(q2 as dynamic));
            Assert.Throws<Microsoft.CSharp.RuntimeBinder.RuntimeBinderException>(() => q1.Subtract(q2 as dynamic));
        }

        [Test]
        public void testArithmetic_DivideByZero_Throws()
        {
            var q1 = new Quantity<LengthUnit>(10, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0, LengthUnit.Feet);

            Assert.Throws<DivideByZeroException>(() => q1.Divide(q2));
        }

        [Test]
        public void testRounding_AddSubtract()
        {
            var q1 = new Quantity<LengthUnit>(1.234, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0.123, LengthUnit.Feet);

            Assert.AreEqual(1.36, q1.Add(q2).Value);
            Assert.AreEqual(1.11, q1.Subtract(q2).Value);
        }

        [Test]
        public void testDivide_NoRounding()
        {
            var q1 = new Quantity<LengthUnit>(10.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(4.0, LengthUnit.Feet);

            Assert.AreEqual(2.5, q1.Divide(q2));
        }
    }
}
