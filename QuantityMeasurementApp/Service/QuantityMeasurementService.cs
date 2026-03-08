using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementService
    {
        // Length Conversion
        private double ConvertLength(double value, LengthUnit from, LengthUnit to)
        {
            // Convert everything to Inch first
            double valueInInch = from switch
            {
                LengthUnit.Feet => value * 12,
                LengthUnit.Yard => value * 36,
                LengthUnit.Cm => value / 2.54,
                LengthUnit.Inch => value,
                _ => throw new Exception("Invalid unit")
            };

            return to switch
            {
                LengthUnit.Feet => valueInInch / 12,
                LengthUnit.Yard => valueInInch / 36,
                LengthUnit.Cm => valueInInch * 2.54,
                LengthUnit.Inch => valueInInch,
                _ => throw new Exception("Invalid unit")
            };
        }

        // Weight Conversion
        private double ConvertWeight(double value, WeightUnit from, WeightUnit to)
        {
            double valueInKg = from switch
            {
                WeightUnit.Kg => value,
                WeightUnit.Gm => value / 1000,
                WeightUnit.Lb => value * 0.453592,
                _ => throw new Exception("Invalid unit")
            };

            return to switch
            {
                WeightUnit.Kg => valueInKg,
                WeightUnit.Gm => valueInKg * 1000,
                WeightUnit.Lb => valueInKg / 0.453592,
                _ => throw new Exception("Invalid unit")
            };
        }

        // Equality
        public bool AreEqual(Quantity<LengthUnit> q1, Quantity<LengthUnit> q2)
        {
            double val1 = ConvertLength(q1.Value, q1.Unit, LengthUnit.Inch);
            double val2 = ConvertLength(q2.Value, q2.Unit, LengthUnit.Inch);
            return Math.Abs(val1 - val2) < 0.0001;
        }

        public bool AreEqual(Quantity<WeightUnit> q1, Quantity<WeightUnit> q2)
        {
            double val1 = ConvertWeight(q1.Value, q1.Unit, WeightUnit.Kg);
            double val2 = ConvertWeight(q2.Value, q2.Unit, WeightUnit.Kg);
            return Math.Abs(val1 - val2) < 0.0001;
        }

        // Generic Equality
        public bool GenericAreEqual<TUnit>(Quantity<TUnit> q1, Quantity<TUnit> q2)
        {
            if (typeof(TUnit) == typeof(LengthUnit))
                return AreEqual(q1 as Quantity<LengthUnit>, q2 as Quantity<LengthUnit>);
            else if (typeof(TUnit) == typeof(WeightUnit))
                return AreEqual(q1 as Quantity<WeightUnit>, q2 as Quantity<WeightUnit>);
            else
                throw new Exception("Unsupported Unit");
        }

        // Conversion
        public Quantity<LengthUnit> Convert(Quantity<LengthUnit> q, LengthUnit targetUnit)
        {
            return new Quantity<LengthUnit>(ConvertLength(q.Value, q.Unit, targetUnit), targetUnit);
        }

        public Quantity<WeightUnit> Convert(Quantity<WeightUnit> q, WeightUnit targetUnit)
        {
            return new Quantity<WeightUnit>(ConvertWeight(q.Value, q.Unit, targetUnit), targetUnit);
        }

        public Quantity<TUnit> GenericConvert<TUnit>(Quantity<TUnit> q, TUnit targetUnit)
        {
            if (typeof(TUnit) == typeof(LengthUnit))
                return new Quantity<TUnit>((TUnit)(object)ConvertLength((double)(object)q.Value, (LengthUnit)(object)q.Unit, (LengthUnit)(object)targetUnit), targetUnit);
            else if (typeof(TUnit) == typeof(WeightUnit))
                return new Quantity<TUnit>((TUnit)(object)ConvertWeight((double)(object)q.Value, (WeightUnit)(object)q.Unit, (WeightUnit)(object)targetUnit), targetUnit);
            else
                throw new Exception("Unsupported Unit");
        }

        // Addition
        public Quantity<LengthUnit> Add(Quantity<LengthUnit> q1, Quantity<LengthUnit> q2)
        {
            double val = ConvertLength(q1.Value, q1.Unit, LengthUnit.Inch) + ConvertLength(q2.Value, q2.Unit, LengthUnit.Inch);
            return new Quantity<LengthUnit>(val, LengthUnit.Inch);
        }

        public Quantity<LengthUnit> Add(Quantity<LengthUnit> q1, Quantity<LengthUnit> q2, LengthUnit targetUnit)
        {
            double val = ConvertLength(q1.Value, q1.Unit, LengthUnit.Inch) + ConvertLength(q2.Value, q2.Unit, LengthUnit.Inch);
            double result = ConvertLength(val, LengthUnit.Inch, targetUnit);
            return new Quantity<LengthUnit>(result, targetUnit);
        }

        public Quantity<WeightUnit> Add(Quantity<WeightUnit> q1, Quantity<WeightUnit> q2, WeightUnit targetUnit)
        {
            double val = ConvertWeight(q1.Value, q1.Unit, WeightUnit.Kg) + ConvertWeight(q2.Value, q2.Unit, WeightUnit.Kg);
            double result = ConvertWeight(val, WeightUnit.Kg, targetUnit);
            return new Quantity<WeightUnit>(result, targetUnit);
        }

        public Quantity<TUnit> GenericAdd<TUnit>(Quantity<TUnit> q1, Quantity<TUnit> q2, TUnit targetUnit)
        {
            if (typeof(TUnit) == typeof(LengthUnit))
            {
                var sum = Add(q1 as Quantity<LengthUnit>, q2 as Quantity<LengthUnit>, (LengthUnit)(object)targetUnit);
                return new Quantity<TUnit>((TUnit)(object)sum.Value, targetUnit);
            }
            else if (typeof(TUnit) == typeof(WeightUnit))
            {
                var sum = Add(q1 as Quantity<WeightUnit>, q2 as Quantity<WeightUnit>, (WeightUnit)(object)targetUnit);
                return new Quantity<TUnit>((TUnit)(object)sum.Value, targetUnit);
            }
            else
                throw new Exception("Unsupported Unit");
        }
    }
}