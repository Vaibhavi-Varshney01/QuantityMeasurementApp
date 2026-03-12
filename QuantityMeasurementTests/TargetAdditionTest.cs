using NUnit.Framework;
using QuantityMeasurementModel.Models;
using QuantityMeasurementRepository;

namespace QuantityMeasurementTests.UC7
{
    [TestFixture]
    public class QuantityMeasurementUC7Tests
    {
        private QuantityMeasurementService service;

        [SetUp]
        public void Setup()
        {
            service = new QuantityMeasurementService();
        }

        // 1
        [Test]
        public void Given1FeetAnd1Feet_WhenAdded_Result2Feet()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = service.Add(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(2));
        }

        // 2
        [Test]
        public void Given1FeetAnd12Inch_WhenAdded_Result2Feet()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = service.Add(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(2).Within(0.001));
        }

        // 3
        [Test]
        public void Given1FeetAnd12Inch_WhenAdded_Result24Inch()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = service.Add(q1, q2, LengthUnit.Inch);

            Assert.That(result.Value, Is.EqualTo(24).Within(0.001));
        }

        // 4
        [Test]
        public void Given2InchAnd2Inch_WhenAdded_Result4Inch()
        {
            var q1 = new Quantity<LengthUnit>(2, LengthUnit.Inch);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Inch);

            var result = service.Add(q1, q2, LengthUnit.Inch);

            Assert.That(result.Value, Is.EqualTo(4));
        }

        // 5
        [Test]
        public void Given3FeetAnd2Feet_WhenAdded_Result5Feet()
        {
            var q1 = new Quantity<LengthUnit>(3, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(2, LengthUnit.Feet);

            var result = service.Add(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(5));
        }

        // 6
        [Test]
        public void Given1FeetAnd1Feet_WhenAdded_Result0_667Yard()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Feet);

            var result = service.Add(q1, q2, LengthUnit.Yard);

            Assert.That(result.Value, Is.EqualTo(0.667).Within(0.001));
        }

        // 7
        [Test]
        public void Given1InchAnd1Inch_WhenAdded_Result5_08Cm()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Inch);
            var q2 = new Quantity<LengthUnit>(1, LengthUnit.Inch);

            var result = service.Add(q1, q2, LengthUnit.Cm);

            Assert.That(result.Value, Is.EqualTo(5.08).Within(0.01));
        }

        // 8
        [Test]
        public void GivenZeroValue_WhenAdded_ResultSameValue()
        {
            var q1 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(0, LengthUnit.Feet);

            var result = service.Add(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(5));
        }

        // 9
        [Test]
        public void GivenNegativeValue_WhenAdded_ResultCorrect()
        {
            var q1 = new Quantity<LengthUnit>(5, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(-2, LengthUnit.Feet);

            var result = service.Add(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(3));
        }

        // 10
        [Test]
        public void Addition_ShouldBeCommutative()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result1 = service.Add(q1, q2, LengthUnit.Feet);
            var result2 = service.Add(q2, q1, LengthUnit.Feet);

            Assert.That(result1.Value, Is.EqualTo(result2.Value).Within(0.001));
        }

        // 11
        [Test]
        public void GivenLargeValues_WhenAdded_ResultCorrect()
        {
            var q1 = new Quantity<LengthUnit>(100, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(200, LengthUnit.Feet);

            var result = service.Add(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(300));
        }

        // 12
        [Test]
        public void ResultUnit_ShouldMatchTargetUnit()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = service.Add(q1, q2, LengthUnit.Yard);

            Assert.That(result.Unit, Is.EqualTo(LengthUnit.Yard));
        }
    }
}
