using System;
using QuantityMeasurementModel.Models;
using QuantityMeasurementRepository;

namespace QuantityMeasurementControllerLayer
{
    public class QuantityMeasurementController
    {
        private readonly QuantityMeasurementService _service;

        public QuantityMeasurementController(QuantityMeasurementService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public bool PerformEqualityDemo()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            return _service.AreEqual(q1, q2);
        }

        public Quantity<LengthUnit> PerformConversionDemo()
        {
            var quantity = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            return _service.GenericConvert(quantity, LengthUnit.Inch);
        }

        public Quantity<LengthUnit> PerformAdditionDemo()
        {
            var q1 = new Quantity<LengthUnit>(1, LengthUnit.Feet);
            var q2 = new Quantity<LengthUnit>(12, LengthUnit.Inch);
            return _service.Add(q1, q2, LengthUnit.Feet);
        }
    }
}
