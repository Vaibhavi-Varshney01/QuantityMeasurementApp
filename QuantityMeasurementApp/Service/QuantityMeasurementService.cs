using System;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp.Service
{
    public class QuantityMeasurementService
    {
        // UC1–UC11: Equality check
        public bool AreEqual<U>(Quantity<U> q1, Quantity<U> q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            if (!typeof(U).Equals(q2.Unit.GetType()))
                throw new ArgumentException("Quantities must be of the same measurement category");

            double baseQ1 = ConvertToBase(q1);
            double baseQ2 = ConvertToBase(q2);
            return Math.Abs(baseQ1 - baseQ2) < 1e-2; // epsilon for rounding
        }

        // UC5–UC11: Addition
        public Quantity<U> Add<U>(Quantity<U> q1, Quantity<U> q2, U targetUnit = default)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            if (!typeof(U).Equals(q2.Unit.GetType()))
                throw new ArgumentException("Quantities must be of the same measurement category");

            double baseSum = ConvertToBase(q1) + ConvertToBase(q2);
            U target = targetUnit.Equals(default(U)) ? q1.Unit : targetUnit;
            double finalValue = ConvertFromBase(baseSum, target);
            return new Quantity<U>(Math.Round(finalValue, 2), target);
        }

        // UC12: Subtraction
        public Quantity<U> Subtract<U>(Quantity<U> q1, Quantity<U> q2, U targetUnit = default)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            if (!typeof(U).Equals(q2.Unit.GetType()))
                throw new ArgumentException("Quantities must be of the same measurement category");

            double baseResult = ConvertToBase(q1) - ConvertToBase(q2);
            U target = targetUnit.Equals(default(U)) ? q1.Unit : targetUnit;
            double finalValue = ConvertFromBase(baseResult, target);
            return new Quantity<U>(Math.Round(finalValue, 2), target);
        }

        // UC12: Division
        public double Divide<U>(Quantity<U> q1, Quantity<U> q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentNullException("Quantities cannot be null");

            if (!typeof(U).Equals(q2.Unit.GetType()))
                throw new ArgumentException("Quantities must be of the same measurement category");

            double divisor = ConvertToBase(q2);
            if (Math.Abs(divisor) < 1e-10)
                throw new DivideByZeroException("Cannot divide by zero quantity");

            return ConvertToBase(q1) / divisor;
        }

        // Helper: Convert to base unit
        private double ConvertToBase<U>(Quantity<U> quantity)
        {
            if (quantity.Unit is LengthUnit l)
                return l switch
                {
                    LengthUnit.Feet => quantity.Value * 12,
                    LengthUnit.Yard => quantity.Value * 36,
                    LengthUnit.Cm => quantity.Value / 2.54,
                    LengthUnit.Inch => quantity.Value,
                    _ => throw new Exception("Invalid LengthUnit")
                };
            else if (quantity.Unit is WeightUnit w)
                return w switch
                {
                    WeightUnit.Kg => quantity.Value,
                    WeightUnit.Gm => quantity.Value / 1000,
                    WeightUnit.Lb => quantity.Value * 0.453592,
                    _ => throw new Exception("Invalid WeightUnit")
                };
            else if (quantity.Unit is VolumeUnit v)
                return v switch
                {
                    VolumeUnit.Litre => quantity.Value,
                    VolumeUnit.Millilitre => quantity.Value / 1000,
                    _ => throw new Exception("Invalid VolumeUnit")
                };
            else
                throw new Exception("Unsupported Unit Type");
        }

        // Helper: Convert from base unit
        private double ConvertFromBase<U>(double baseValue, U unit)
        {
            if (unit is LengthUnit l)
                return l switch
                {
                    LengthUnit.Feet => baseValue / 12,
                    LengthUnit.Yard => baseValue / 36,
                    LengthUnit.Cm => baseValue * 2.54,
                    LengthUnit.Inch => baseValue,
                    _ => throw new Exception("Invalid LengthUnit")
                };
            else if (unit is WeightUnit w)
                return w switch
                {
                    WeightUnit.Kg => baseValue,
                    WeightUnit.Gm => baseValue * 1000,
                    WeightUnit.Lb => baseValue / 0.453592,
                    _ => throw new Exception("Invalid WeightUnit")
                };
            else if (unit is VolumeUnit v)
                return v switch
                {
                    VolumeUnit.Litre => baseValue,
                    VolumeUnit.Millilitre => baseValue * 1000,
                    _ => throw new Exception("Invalid VolumeUnit")
                };
            else
                throw new Exception("Unsupported Unit Type");
        }
    }
}