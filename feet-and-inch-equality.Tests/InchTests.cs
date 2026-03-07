using NUnit.Framework;
using feet_and_inches_equality;

namespace feet_and_inches_equality.Tests
{
    [TestFixture]
    public class InchesTests
    {
        [Test]
        public void testEquality_SameValue()
        {
            var i1 = new Inches(12);
            var i2 = new Inches(12);

            Assert.That(i1.Equals(i2), Is.True);
        }

        [Test]
        public void testEquality_DifferentValue()
        {
            var i1 = new Inches(12);
            var i2 = new Inches(24);

            Assert.That(i1.Equals(i2), Is.False);
        }

        [Test]
        public void testEquality_NullComparison()
        {
            var i1 = new Inches(12);

            Assert.That(i1.Equals(null), Is.False);
        }

        [Test]
        public void testEquality_NonNumericInput()
        {
            var i1 = new Inches(12);

            Assert.That(i1.Equals(123), Is.False);
        }

        [Test]
        public void testEquality_SameReference()
        {
            var i1 = new Inches(12);
            var sameRef = i1;

            Assert.That(i1.Equals(sameRef), Is.True);
        }
    }
}