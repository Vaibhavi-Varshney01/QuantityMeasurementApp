using NUnit.Framework;
using feet_and_inches_equality;

namespace feet_and_inches_equality.Tests
{
    [TestFixture]
    public class FeetTests
    {
        [Test]
        public void testEquality_SameValue()
        {
            var f1 = new Feet(10);
            var f2 = new Feet(10);

            Assert.That(f1.Equals(f2), Is.True);
        }

        [Test]
        public void testEquality_DifferentValue()
        {
            var f1 = new Feet(10);
            var f2 = new Feet(20);

            Assert.That(f1.Equals(f2), Is.False);
        }

        [Test]
        public void testEquality_NullComparison()
        {
            var f1 = new Feet(10);

            Assert.That(f1.Equals(null), Is.False);
        }

        [Test]
        public void testEquality_NonNumericInput()
        {
            var f1 = new Feet(10);

            Assert.That(f1.Equals("hello"), Is.False);
        }

        [Test]
        public void testEquality_SameReference()
        {
            var f1 = new Feet(10);
            var sameRef = f1;

            Assert.That(f1.Equals(sameRef), Is.True);
        }
    }
}