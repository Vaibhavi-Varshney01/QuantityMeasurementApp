using NUnit.Framework;
using feet_measurement_equality;

namespace feet_measurement_equality.Tests{
    public class FeetTests{

        [Test]
        public void GivenSameValues(){
            Feet first = new Feet(1.0);
            Feet second = new Feet(1.0);

            bool result = first.Equals(second);

            Assert.That(result, Is.True);
        }

        [Test]
        public void GivenDifferentValues(){
            Feet first = new Feet(1.0);
            Feet second = new Feet(2.0);

            bool result = first.Equals(second);

            Assert.That(result, Is.False);
        }

        [Test]
        public void GivenNonFeetObject(){
            Feet first = new Feet(1.0);
            object obj = new object();

            bool result = first.Equals(obj);

            Assert.That(result, Is.False);
        }
        
        [Test]
        public void GivenNegativeValue_WhenCreatingFeet_ShouldThrowException(){
            Assert.That(() => new Feet(-1.0),
                Throws.TypeOf<InvalidFeetException>());
        }
    }
}