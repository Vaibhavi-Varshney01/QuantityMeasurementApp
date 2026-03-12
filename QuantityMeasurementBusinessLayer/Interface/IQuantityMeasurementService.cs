// using QuantityMeasurementRepository;
using QuantityMeasurementBusinessLayer.DTO;

namespace QuantityMeasurementBusinessLayer
{
    public interface IQuantityMeasurementService
    {
        QuantityDTO Compare(QuantityDTO q1, QuantityDTO q2);
        QuantityDTO Convert(QuantityDTO q, string targetUnit);
        QuantityDTO Add(QuantityDTO q1, QuantityDTO q2);
        QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2);
        QuantityDTO Divide(QuantityDTO q1, QuantityDTO q2);
    }
}