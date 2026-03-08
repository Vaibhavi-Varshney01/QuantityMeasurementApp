using System;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            QuantityMeasurementService service = new QuantityMeasurementService();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== Quantity Measurement App UC1–UC6 ===");
                Console.WriteLine("1. Compare Feet (UC1)");
                Console.WriteLine("2. Compare Inches (UC2)");
                Console.WriteLine("3. Compare QuantityLength (UC3 + UC4)");
                Console.WriteLine("4. Convert Units (UC5)");
                Console.WriteLine("5. Add Two Lengths (UC6)");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CompareFeet(service);
                        break;
                    case "2":
                        CompareInches(service);
                        break;
                    case "3":
                        CompareQuantity(service);
                        break;
                    case "4":
                        ConvertUnits(service);
                        break;
                    case "5":
                        AddLengths(service);
                        break;
                    case "6":
                        exit = true;
                        Console.WriteLine("Exiting app...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }
            }
        }

        // ===== UC1 =====
        private static void CompareFeet(QuantityMeasurementService service)
        {
            Console.Write("Enter first value in feet: ");
            double f1Val = GetDoubleInput();
            Console.Write("Enter second value in feet: ");
            double f2Val = GetDoubleInput();

            Feet f1 = new Feet(f1Val);
            Feet f2 = new Feet(f2Val);

            bool result = service.AreFeetEqual(f1, f2);
            Console.WriteLine($"Feet Equal? {result}");
        }

        // ===== UC2 =====
        private static void CompareInches(QuantityMeasurementService service)
        {
            Console.Write("Enter first value in inches: ");
            double i1Val = GetDoubleInput();
            Console.Write("Enter second value in inches: ");
            double i2Val = GetDoubleInput();

            Inches i1 = new Inches(i1Val);
            Inches i2 = new Inches(i2Val);

            bool result = service.AreInchesEqual(i1, i2);
            Console.WriteLine($"Inches Equal? {result}");
        }

        // ===== UC3 + UC4 =====
        private static void CompareQuantity(QuantityMeasurementService service)
        {
            Console.WriteLine("Supported units: Feet, Inch, Yard, Cm");
            Console.Write("Enter first value: ");
            double val1 = GetDoubleInput();
            LengthUnit unit1 = GetUnitInput();

            Console.Write("Enter second value: ");
            double val2 = GetDoubleInput();
            LengthUnit unit2 = GetUnitInput();

            QuantityLength q1 = new QuantityLength(val1, unit1);
            QuantityLength q2 = new QuantityLength(val2, unit2);

            bool result = service.AreEqual(q1, q2);
            Console.WriteLine($"Are Quantities Equal? {result}");
        }

        // ===== UC5 =====
        private static void ConvertUnits(QuantityMeasurementService service)
        {
            Console.WriteLine("Supported units: Feet, Inch, Yard, Cm");
            Console.Write("Enter value to convert: ");
            double val = GetDoubleInput();
            Console.Write("Enter source unit: ");
            LengthUnit sourceUnit = GetUnitInput();
            Console.Write("Enter target unit: ");
            LengthUnit targetUnit = GetUnitInput();

            try
            {
                double converted = service.Convert(val, sourceUnit, targetUnit);
                Console.WriteLine($"{val} {sourceUnit} = {converted} {targetUnit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // ===== UC6 =====
        private static void AddLengths(QuantityMeasurementService service)
        {
            Console.WriteLine("Supported units: Feet, Inch, Yard, Cm");
            Console.Write("Enter first value: ");
            double val1 = GetDoubleInput();
            LengthUnit unit1 = GetUnitInput();

            Console.Write("Enter second value: ");
            double val2 = GetDoubleInput();
            LengthUnit unit2 = GetUnitInput();

            QuantityLength q1 = new QuantityLength(val1, unit1);
            QuantityLength q2 = new QuantityLength(val2, unit2);

            try
            {
                QuantityLength sum = service.Add(q1, q2);
                Console.WriteLine($"Sum: {sum.Value} {sum.Unit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Helper methods
        private static double GetDoubleInput()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (double.TryParse(input, out double value))
                    return value;
                Console.Write("Invalid input! Enter a numeric value: ");
            }
        }

        private static LengthUnit GetUnitInput()
        {
            while (true)
            {
                string input = Console.ReadLine().Trim().ToLower();
                return input switch
                {
                    "feet" => LengthUnit.Feet,
                    "inch" => LengthUnit.Inch,
                    "yard" => LengthUnit.Yard,
                    "cm" => LengthUnit.Cm,
                    _ => throw new ArgumentException("Unsupported unit type")
                };
            }
        }
    }
}