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
                Console.WriteLine("\n=== Quantity Measurement App ===");
                Console.WriteLine("1. Compare Feet");
                Console.WriteLine("2. Compare Inches");
                Console.WriteLine("3. Exit");
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
                        exit = true;
                        Console.WriteLine("Exiting app...");
                        break;
                    default:
                        Console.WriteLine("Invalid choice! Try again.");
                        break;
                }
            }
        }

        // Feet comparison
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

        // Inches comparison
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

        // Input helper
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
    }
}