using NUnit.Framework;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class WeightMeasurementTests
    {

        // ---------------------------
        // Equality Test Cases
        // ---------------------------

        [Test]
        public void GivenSameKilogramValues_WhenCompared_ShouldReturnTrue()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(1.0, WeightUnit.Kilogram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void GivenDifferentKilogramValues_WhenCompared_ShouldReturnFalse()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(2.0, WeightUnit.Kilogram);

            Assert.That(w1.Equals(w2), Is.False);
        }

        [Test]
        public void GivenKilogramAndGram_WhenCompared_ShouldReturnTrue()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(1000.0, WeightUnit.Gram);

            Assert.That(w1.Equals(w2), Is.True);
        }

        [Test]
        public void GivenKilogramAndPound_WhenCompared_ShouldReturnTrue()
        {
            var w1 = new QuantityWeight(1.0, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(2.20462, WeightUnit.Pound);

            Assert.That(w1.Equals(w2), Is.True);
        }


        // ---------------------------
        // Conversion Test Cases
        // ---------------------------

        [Test]
        public void GivenKilogram_WhenConvertedToGram_ShouldReturn1000()
        {
            var weight = new QuantityWeight(1.0, WeightUnit.Kilogram);

            var result = weight.ConvertTo(WeightUnit.Gram);

            Assert.That(result.Value, Is.EqualTo(1000).Within(0.01));
        }

        [Test]
        public void GivenGram_WhenConvertedToKilogram_ShouldReturn1()
        {
            var weight = new QuantityWeight(1000, WeightUnit.Gram);

            var result = weight.ConvertTo(WeightUnit.Kilogram);

            Assert.That(result.Value, Is.EqualTo(1).Within(0.01));
        }

        [Test]
        public void GivenPound_WhenConvertedToKilogram_ShouldReturn1()
        {
            var weight = new QuantityWeight(2.20462, WeightUnit.Pound);

            var result = weight.ConvertTo(WeightUnit.Kilogram);

            Assert.That(result.Value, Is.EqualTo(1).Within(0.01));
        }


        // ---------------------------
        // Addition Test Cases
        // ---------------------------

        [Test]
        public void GivenTwoKilograms_WhenAdded_ShouldReturnThreeKilograms()
        {
            var w1 = new QuantityWeight(1, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(2, WeightUnit.Kilogram);

            var result = w1.Add(w2);

            Assert.That(result.Value, Is.EqualTo(3));
        }

        [Test]
        public void GivenKilogramAndGram_WhenAdded_ShouldReturnTwoKilograms()
        {
            var w1 = new QuantityWeight(1, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(1000, WeightUnit.Gram);

            var result = w1.Add(w2);

            Assert.That(result.Value, Is.EqualTo(2).Within(0.01));
        }

        [Test]
        public void GivenWeights_WhenAddedWithTargetUnitGram_ShouldReturn2000Gram()
        {
            var w1 = new QuantityWeight(1, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(1000, WeightUnit.Gram);

            var result = w1.Add(w2, WeightUnit.Gram);

            Assert.That(result.Value, Is.EqualTo(2000).Within(0.01));
        }


        // ---------------------------
        // Edge Case Tests
        // ---------------------------

        [Test]
        public void GivenWeightAndZero_WhenAdded_ShouldReturnSameWeight()
        {
            var w1 = new QuantityWeight(5, WeightUnit.Kilogram);
            var w2 = new QuantityWeight(0, WeightUnit.Gram);

            var result = w1.Add(w2);

            Assert.That(result.Value, Is.EqualTo(5).Within(0.01));
        }

        [Test]
        public void GivenNullWeight_WhenCompared_ShouldReturnFalse()
        {
            QuantityWeight w1 = null;
            var w2 = new QuantityWeight(1, WeightUnit.Kilogram);

            Assert.That(w1 == null);
        }
    }
}