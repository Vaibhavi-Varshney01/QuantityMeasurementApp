using NUnit.Framework;

namespace QuantityMeasurementTests
{
    public class YardEqualityTests
    {
        // Yard to Yard

        [Test]
        public void testEquality_YardToYard_SameValue()
        {
            yard_equality.QuantityLength q1 = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);
            yard_equality.QuantityLength q2 = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);

            Assert.That(q1.Equals(q2), Is.True);
        }

        [Test]
        public void testEquality_YardToYard_DifferentValue()
        {
            yard_equality.QuantityLength q1 = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);
            yard_equality.QuantityLength q2 = new yard_equality.QuantityLength(2.0, yard_equality.LengthUnit.YARDS);

            Assert.That(q1.Equals(q2), Is.False);
        }

        // Yard to Feet

        [Test]
        public void testEquality_YardToFeet_EquivalentValue()
        {
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);
            yard_equality.QuantityLength feet = new yard_equality.QuantityLength(3.0, yard_equality.LengthUnit.FEET);

            Assert.That(yard.Equals(feet), Is.True);
        }

        [Test]
        public void testEquality_FeetToYard_EquivalentValue()
        {
            yard_equality.QuantityLength feet = new yard_equality.QuantityLength(3.0, yard_equality.LengthUnit.FEET);
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);

            Assert.That(feet.Equals(yard), Is.True);
        }

        // Yard to Inches

        [Test]
        public void testEquality_YardToInches_EquivalentValue()
        {
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);
            yard_equality.QuantityLength inch = new yard_equality.QuantityLength(36.0, yard_equality.LengthUnit.INCHES);

            Assert.That(yard.Equals(inch), Is.True);
        }

        [Test]
        public void testEquality_InchesToYard_EquivalentValue()
        {
            yard_equality.QuantityLength inch = new yard_equality.QuantityLength(36.0, yard_equality.LengthUnit.INCHES);
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);

            Assert.That(inch.Equals(yard), Is.True);
        }

        [Test]
        public void testEquality_YardToFeet_NonEquivalentValue()
        {
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);
            yard_equality.QuantityLength feet = new yard_equality.QuantityLength(2.0, yard_equality.LengthUnit.FEET);

            Assert.That(yard.Equals(feet), Is.False);
        }

        // CM conversions

        [Test]
        public void testEquality_centimetersToInches_EquivalentValue()
        {
            yard_equality.QuantityLength cm = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.CENTIMETERS);
            yard_equality.QuantityLength inch = new yard_equality.QuantityLength(0.393701, yard_equality.LengthUnit.INCHES);

            Assert.That(cm.Equals(inch), Is.True);
        }

        [Test]
        public void testEquality_centimetersToFeet_NonEquivalentValue()
        {
            yard_equality.QuantityLength cm = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.CENTIMETERS);
            yard_equality.QuantityLength feet = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.FEET);

            Assert.That(cm.Equals(feet), Is.False);
        }

        // Transitive property

        [Test]
        public void testEquality_MultiUnit_TransitiveProperty()
        {
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);
            yard_equality.QuantityLength feet = new yard_equality.QuantityLength(3.0, yard_equality.LengthUnit.FEET);
            yard_equality.QuantityLength inch = new yard_equality.QuantityLength(36.0, yard_equality.LengthUnit.INCHES);

            Assert.That(yard.Equals(feet), Is.True);
            Assert.That(feet.Equals(inch), Is.True);
            Assert.That(yard.Equals(inch), Is.True);
        }

        // Null unit tests

        [Test]
        public void testEquality_YardWithNullUnit()
        {
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);

            Assert.That(yard.Equals(null), Is.False);
        }

        [Test]
        public void testEquality_YardSameReference()
        {
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);

            Assert.That(yard.Equals(yard), Is.True);
        }

        [Test]
        public void testEquality_YardNullComparison()
        {
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.YARDS);

            Assert.That(yard.Equals(null), Is.False);
        }

        // CM null tests

        [Test]
        public void testEquality_CentimetersSameReference()
        {
            yard_equality.QuantityLength cm = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.CENTIMETERS);

            Assert.That(cm.Equals(cm), Is.True);
        }

        [Test]
        public void testEquality_CentimetersNullComparison()
        {
            yard_equality.QuantityLength cm = new yard_equality.QuantityLength(1.0, yard_equality.LengthUnit.CENTIMETERS);

            Assert.That(cm.Equals(null), Is.False);
        }

        // Complex scenario

        [Test]
        public void testEquality_AllUnits_ComplexScenario()
        {
            yard_equality.QuantityLength yard = new yard_equality.QuantityLength(2.0, yard_equality.LengthUnit.YARDS);
            yard_equality.QuantityLength feet = new yard_equality.QuantityLength(6.0, yard_equality.LengthUnit.FEET);
            yard_equality.QuantityLength inches = new yard_equality.QuantityLength(72.0, yard_equality.LengthUnit.INCHES);

            Assert.That(yard.Equals(feet), Is.True);
            Assert.That(feet.Equals(inches), Is.True);
            Assert.That(yard.Equals(inches), Is.True);
        }
    }
}
