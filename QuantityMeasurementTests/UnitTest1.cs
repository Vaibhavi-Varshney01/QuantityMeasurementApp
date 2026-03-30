using NUnit.Framework;
using QuantityMeasurementModel.Models;
using QuantityMeasurementRepository;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class QuantityMeasurementTests
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // ===== UC1: Feet =====
        [Test]
        public void TestEquality_SameValue_Feet()
        {
            Feet f1 = new Feet(5.0);
            Feet f2 = new Feet(5.0);
            Assert.That(service.AreFeetEqual(f1, f2), Is.True);
        }

        [Test]
        public void TestEquality_DifferentValue_Feet()
        {
            Feet f1 = new Feet(5.0);
            Feet f2 = new Feet(6.0);
            Assert.That(service.AreFeetEqual(f1, f2), Is.False);
        }

        [Test]
        public void TestEquality_NullComparison_Feet()
        {
            Feet f1 = new Feet(5.0);
            Feet? f2 = null;
            Assert.That(service.AreFeetEqual(f1, f2!), Is.False);
        }

        // ===== UC2: Inches =====
        [Test]
        public void TestEquality_SameValue_Inches()
        {
            Inches i1 = new Inches(12.0);
            Inches i2 = new Inches(12.0);
            Assert.That(service.AreInchesEqual(i1, i2), Is.True);
        }

        [Test]
        public void TestEquality_DifferentValue_Inches()
        {
            Inches i1 = new Inches(12.0);
            Inches i2 = new Inches(10.0);
            Assert.That(service.AreInchesEqual(i1, i2), Is.False);
        }

        [Test]
        public void TestEquality_NullComparison_Inches()
        {
            Inches i1 = new Inches(12.0);
            Inches? i2 = null;
            Assert.That(service.AreInchesEqual(i1, i2!), Is.False);
        }

       // ===== UC2: Feet vs Inches =====
[Test]
public void TestEquality_FeetToInch_EquivalentValue()
{
    Quantity<LengthUnit> q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
    Quantity<LengthUnit> q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);

    Assert.That(service.AreEqual(q1, q2), Is.True);
}

[Test]
public void TestEquality_FeetToInch_DifferentValue()
{
    Quantity<LengthUnit> q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
    Quantity<LengthUnit> q2 = new Quantity<LengthUnit>(10.0, LengthUnit.Inch);

    Assert.That(service.AreEqual(q1, q2), Is.False);
}

        // ===== UC3: Generic Quantity =====
        [Test]
        public void TestEquality_Quantity_SameUnit()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_Quantity_CrossUnit()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12.0, LengthUnit.Inch);
            Assert.That(service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void TestEquality_Quantity_DifferentValue()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2.0, LengthUnit.Feet);
            Assert.That(service.AreEqual(q1, q2), Is.False);
        }

        [Test]
        public void TestEquality_Quantity_NullComparison()
        {
            var q1 = new Quantity<LengthUnit>(1.0, LengthUnit.Feet);
            Quantity<LengthUnit>? q2 = null;
            Assert.That(service.AreEqual(q1, q2!), Is.False);
        }
    }
}
