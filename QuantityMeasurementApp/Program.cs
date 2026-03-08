using System;
using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Service;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main()
        {
            QuantityMeasurementService service = new QuantityMeasurementService();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== Quantity Measurement App ===");
                Console.WriteLine("1. Compare Feet (UC1)");
                Console.WriteLine("2. Compare Inches (UC2)");
                Console.WriteLine("3. Compare Quantity (UC3 + UC4)");
                Console.WriteLine("4. Exit");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": CompareFeet(service); break;
                    case "2": CompareInches(service); break;
                    case "3": CompareQuantity(service); break;
                    case "4": exit = true; break;
                    default: Console.WriteLine("Invalid choice!"); break;
                }
            }
        }

        private static void CompareFeet(QuantityMeasurementService service)
        {
            Console.Write("Enter first feet: "); double f1 = GetDouble();
            Console.Write("Enter second feet: "); double f2 = GetDouble();
            Console.WriteLine($"Feet Equal? {service.AreFeetEqual(new Feet(f1), new Feet(f2))}");
        }

        private static void CompareInches(QuantityMeasurementService service)
        {
            Console.Write("Enter first inches: "); double i1 = GetDouble();
            Console.Write("Enter second inches: "); double i2 = GetDouble();
            Console.WriteLine($"Inches Equal? {service.AreInchesEqual(new Inches(i1), new Inches(i2))}");
        }

        private static void CompareQuantity(QuantityMeasurementService service)
        {
            Console.WriteLine("Supported units: Feet, Inch, Yard, Cm");
            Console.Write("Enter first value: "); double val1 = GetDouble();
            Console.Write("Enter first unit: "); LengthUnit u1 = GetUnit();
            Console.Write("Enter second value: "); double val2 = GetDouble();
            Console.Write("Enter second unit: "); LengthUnit u2 = GetUnit();

            Console.WriteLine($"Quantities Equal? {service.AreEqual(new QuantityLength(val1, u1), new QuantityLength(val2, u2))}");
        }

        private static double GetDouble()
        {
            while (!double.TryParse(Console.ReadLine(), out double val))
                Console.Write("Invalid! Enter numeric value: ");
            return double.Parse(Console.ReadLine());
        }

        private static LengthUnit GetUnit()
        {
            string input = Console.ReadLine().Trim().ToLower();
            return input switch
            {
                "feet" => LengthUnit.Feet,
                "inch" => LengthUnit.Inch,
                "yard" => LengthUnit.Yard,
                "cm" => LengthUnit.Cm,
                _ => throw new ArgumentException("Unsupported unit")
            };
        }
    }
}