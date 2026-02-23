
// Description: NUnit test cases for QuantityLength class
// Purpose: Verifies equality, invalid unit handling, and cross-unit comparison
// Key Test Cases:
//  - testEquality_FeetToFeet_SameValue
//  - testEquality_InchToInch_SameValue
//  - testEquality_InchToFeet_EquivalentValue
//  - testEquality_InvalidUnit
//  - testEquality_NullComparison

using NUnit.Framework;
using generic_length;

namespace generic_length.Tests
{
    [TestFixture]
    public class QuantityLengthTests
    {

        // Checks if two feet values are equal
        [Test]
        public void testEquality_FeetToFeet_SameValue()
        {
            var length1 = new QuantityLength(1.0, LengthUnit.FEET);
            var length2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(length1.Equals(length2));
        }

        // Checks if two inch values are equal
        [Test]
        public void testEquality_InchToInch_SameValue()
        {
            var length1 = new QuantityLength(1.0, LengthUnit.INCH);
            var length2 = new QuantityLength(1.0, LengthUnit.INCH);

            Assert.IsTrue(length1.Equals(length2));
        }

        // Checks if equivalent values in different units are equal
        [Test]
        public void testEquality_FeetToInch_EquivalentValue()
        {
            var length1 = new QuantityLength(1.0, LengthUnit.FEET);
            var length2 = new QuantityLength(12.0, LengthUnit.INCH);

            Assert.IsTrue(length1.Equals(length2));
        }

        [Test]
        public void testEquality_InchToFeet_EquivalentValue()
        {
            var length1 = new QuantityLength(12.0, LengthUnit.INCH);
            var length2 = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsTrue(length1.Equals(length2));
        }

        [Test]
        public void testEquality_FeetToFeet_DifferentValue()
        {
            var length1 = new QuantityLength(1.0, LengthUnit.FEET);
            var length2 = new QuantityLength(2.0, LengthUnit.FEET);

            Assert.IsFalse(length1.Equals(length2));
        }

        [Test]
        public void testEquality_InchToInch_DifferentValue()
        {
            var length1 = new QuantityLength(1.0, LengthUnit.INCH);
            var length2 = new QuantityLength(2.0, LengthUnit.INCH);

            Assert.IsFalse(length1.Equals(length2));
        }


        // Checks exception thrown for invalid units
        [Test]
        public void testEquality_InvalidUnit()
        {
            Assert.Throws<InvalidLengthException>(() =>
                new QuantityLength(1.0, (LengthUnit)100)
            );
        }

        // Checks the test equality on null units
        [Test]
        public void testEquality_NullUnit()
        {
            Assert.Throws<InvalidLengthException>(() =>
                new QuantityLength(1.0, null!)
            );
        }
        // Checks test equality when the reference is same
        [Test]
        public void testEquality_SameReference()
        {
            var length = new QuantityLength(5.0, LengthUnit.FEET);

            Assert.IsTrue(length.Equals(length));
        }

        // Checks behavior when one object is null
        [Test]
        public void testEquality_NullComparison()
        {
            var length = new QuantityLength(1.0, LengthUnit.FEET);

            Assert.IsFalse(length.Equals(null));
        }
    }
}