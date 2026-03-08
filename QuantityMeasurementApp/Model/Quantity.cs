using QuantityMeasurementApp.Unit;

namespace QuantityMeasurementApp.Model
{
    public class Quantity<U> where U : struct
    {
        public double Value { get; }
        public U Unit { get; }

        public Quantity(double value, U unit)
        {
            if (!value.IsFinite())
                throw new ArgumentException("Value must be finite");
            this.Unit = unit;
            this.Value = value;
        }

        public Quantity<U> ConvertTo(U targetUnit)
        {
            dynamic src = Unit;
            dynamic tgt = targetUnit;

            double baseValue = src.ConvertToBaseUnit(Value);
            double result = tgt.ConvertFromBaseUnit(baseValue);
            return new Quantity<U>(Math.Round(result, 2), targetUnit);
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            return Add(other, Unit);
        }

        public Quantity<U> Add(Quantity<U> other, U targetUnit)
        {
            dynamic src1 = Unit;
            dynamic src2 = other.Unit;
            dynamic tgt = targetUnit;

            double sumBase = src1.ConvertToBaseUnit(Value) + src2.ConvertToBaseUnit(other.Value);
            double result = tgt.ConvertFromBaseUnit(sumBase);
            return new Quantity<U>(Math.Round(result, 2), targetUnit);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;

            var that = (Quantity<U>)obj;
            dynamic src1 = Unit;
            dynamic src2 = that.Unit;

            double val1 = src1.ConvertToBaseUnit(Value);
            double val2 = src2.ConvertToBaseUnit(that.Value);

            return Math.Abs(val1 - val2) < 0.0001;
        }

        public override int GetHashCode()
        {
            dynamic src = Unit;
            double baseVal = src.ConvertToBaseUnit(Value);
            return baseVal.GetHashCode();
        }

        public override string ToString() => $"Quantity({Value}, {Unit})";
    }

    static class DoubleExtensions
    {
        public static bool IsFinite(this double d) => !double.IsNaN(d) && !double.IsInfinity(d);
    }
}