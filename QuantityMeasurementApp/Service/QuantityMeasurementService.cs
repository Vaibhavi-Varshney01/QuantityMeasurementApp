using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementService
    {
        public bool AreFeetEqual(Feet first, Feet second)
        {
            return first.Equals(second);
        }
    }
}