namespace QuantityMeasurementApp.Model
{
    public class QuantityWeight
    {
        public double Value { get; }
        public WeightUnit Unit { get; }

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value must be finite", nameof(value));

            Value = value;
            Unit = unit;
        }

        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = Unit.ConvertToBaseUnit(Value);
            double converted = targetUnit.ConvertFromBaseUnit(baseValue);
            return new QuantityWeight(converted, targetUnit);
        }

        public QuantityWeight Add(QuantityWeight other)
        {
            return Add(other, Unit);
        }

        public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);
            double sumBase = base1 + base2;
            double result = targetUnit.ConvertFromBaseUnit(sumBase);

            return new QuantityWeight(result, targetUnit);
        }

        public override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != this.GetType()) return false;

            var other = (QuantityWeight)obj;
            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);
            return Math.Abs(base1 - base2) < 0.01;
        }

        public override int GetHashCode()
        {
            return Unit.ConvertToBaseUnit(Value).GetHashCode();
        }
    }
}
