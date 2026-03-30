using QuantityMeasurementRepository;
using QuantityMeasurementModel.Models;

namespace QuantityMeasurementTests
{
    [TestFixture]
    public class ServiceWithRepositoryTest
    {
        private IQuantityMeasurementRepository _repo = null!;
        private QuantityMeasurementService _service = null!;

        [SetUp]
        public void Setup()
        {
            _repo = QuantityMeasurementCacheRepository.Instance;
            _repo.DeleteAll();
            _service = new QuantityMeasurementService();
        }

        [Test]
        public void testService_LengthEquality_Success()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            Assert.That(_service.AreEqual(q1, q2), Is.True);
        }

        [Test]
        public void testService_LengthAddition_Success()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);

            var result = _service.Add(q1, q2, LengthUnit.Feet);

            Assert.That(result.Value, Is.EqualTo(2));
        }

        [Test]
        public void testService_BothRepos_ImplementSameInterface()
        {
            IQuantityMeasurementRepository cache = QuantityMeasurementCacheRepository.Instance;
            Assert.That(cache, Is.InstanceOf<IQuantityMeasurementRepository>());
        }
    }
}