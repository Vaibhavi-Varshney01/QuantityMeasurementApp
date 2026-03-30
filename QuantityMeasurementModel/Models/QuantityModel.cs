namespace QuantityMeasurementModel
{
    public class QuantityModel<U>
    {
        public double Value { get; set; }
        public U? Unit { get; set; }
    }
}
