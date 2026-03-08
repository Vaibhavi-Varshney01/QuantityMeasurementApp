using System;

namespace QuantityMeasurementApp.Model
{
    public class QuantityWeight
    {
        public double Value { get; }
        public WeightUnit Unit { get; }

        public QuantityWeight(double value, WeightUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Invalid weight value");

            Value = value;
            Unit = unit;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != typeof(QuantityWeight))
                return false;

            QuantityWeight other = (QuantityWeight)obj;

            double val1 = Unit.ConvertToBaseUnit(Value);
            double val2 = other.Unit.ConvertToBaseUnit(other.Value);

            return Math.Abs(val1 - val2) < 0.0001;
        }

        public override int GetHashCode()
        {
            return Unit.ConvertToBaseUnit(Value).GetHashCode();
        }

        public QuantityWeight ConvertTo(WeightUnit targetUnit)
        {
            double baseValue = Unit.ConvertToBaseUnit(Value);
            double result = targetUnit.ConvertFromBaseUnit(baseValue);

            return new QuantityWeight(result, targetUnit);
        }

        public QuantityWeight Add(QuantityWeight other)
        {
            return Add(other, this.Unit);
        }

        public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
        {
            if (other == null)
                throw new ArgumentException("Weight cannot be null");

            double base1 = Unit.ConvertToBaseUnit(Value);
            double base2 = other.Unit.ConvertToBaseUnit(other.Value);

            double sum = base1 + base2;

            double result = targetUnit.ConvertFromBaseUnit(sum);

            return new QuantityWeight(result, targetUnit);
        }

        public override string ToString()
        {
            return $"Quantity({Value}, {Unit})";
        }
    }
}