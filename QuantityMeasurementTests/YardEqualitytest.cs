using NUnit.Framework;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class QuantityMeasurementTestsUC4
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // ===== UC4: Yard-to-Yard =====
        [Test]
        public void TestEquality_YardToYard_SameValue()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(1.0, LengthUnit.Yard);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_YardToYard_DifferentValue()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(2.0, LengthUnit.Yard);
            Assert.That(service.AreEqual(q1, q2), Is.False);
        }

        // ===== UC4: Yard to Feet/Inches =====
        [Test]
        public void TestEquality_YardToFeet_EquivalentValue()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(3.0, LengthUnit.Feet);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_FeetToYard_EquivalentValue()
        {
            QuantityLength q1 = new QuantityLength(3.0, LengthUnit.Feet);
            QuantityLength q2 = new QuantityLength(1.0, LengthUnit.Yard);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_YardToInches_EquivalentValue()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(36.0, LengthUnit.Inch);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_InchesToYard_EquivalentValue()
        {
            QuantityLength q1 = new QuantityLength(36.0, LengthUnit.Inch);
            QuantityLength q2 = new QuantityLength(1.0, LengthUnit.Yard);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_YardToFeet_NonEquivalentValue()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(2.0, LengthUnit.Feet);
            Assert.That(service.AreEqual(q1, q2), Is.False);
        }

        // ===== UC4: Centimeters =====
        [Test]
        public void TestEquality_CentimetersToInches_EquivalentValue()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Cm);
            QuantityLength q2 = new QuantityLength(0.393701, LengthUnit.Inch);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_CentimetersToFeet_NonEquivalentValue()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Cm);
            QuantityLength q2 = new QuantityLength(1.0, LengthUnit.Feet);
            Assert.That(service.AreEqual(q1, q2), Is.False);
        }

        [Test]
        public void TestEquality_CentimetersToCentimeters_SameValue()
        {
            QuantityLength q1 = new QuantityLength(2.0, LengthUnit.Cm);
            QuantityLength q2 = new QuantityLength(2.0, LengthUnit.Cm);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_CentimetersToCentimeters_DifferentValue()
        {
            QuantityLength q1 = new QuantityLength(2.0, LengthUnit.Cm);
            QuantityLength q2 = new QuantityLength(3.0, LengthUnit.Cm);
            Assert.That(service.AreEqual(q1, q2), Is.False);
        }

        // ===== UC4: Multi-unit transitive property =====
        [Test]
        public void TestEquality_MultiUnit_TransitiveProperty()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength q2 = new QuantityLength(3.0, LengthUnit.Feet);
            QuantityLength q3 = new QuantityLength(36.0, LengthUnit.Inch);

            Assert.That(service.AreEqual(q1, q2), Is.True);
            Assert.That(service.AreEqual(q2, q3), Is.True);
            Assert.That(service.AreEqual(q1, q3), Is.True);
        }

        // ===== UC4: Null and same reference checks =====
        [Test]
        public void TestEquality_YardSameReference()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength q2 = q1;
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_YardNullComparison()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Yard);
            QuantityLength? q2 = null;
            Assert.That(service.AreEqual(q1, q2!), Is.False);
        }

        [Test]
        public void TestEquality_CentimetersSameReference()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Cm);
            QuantityLength q2 = q1;
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_CentimetersNullComparison()
        {
            QuantityLength q1 = new QuantityLength(1.0, LengthUnit.Cm);
            QuantityLength? q2 = null;
            Assert.That(service.AreEqual(q1, q2!), Is.False);
        }
    }
}