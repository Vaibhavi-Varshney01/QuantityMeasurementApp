using System;

namespace yard_equality
{
    class Program
    {
        static LengthUnit GetUnit(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new InvalidUnitException("Unit cannot be empty");

            input = input.ToUpper();

            switch (input)
            {
                case "INCHES":
                    return LengthUnit.INCHES;

                case "FEET":
                    return LengthUnit.FEET;

                case "YARDS":
                    return LengthUnit.YARDS;

                case "CENTIMETERS":
                    return LengthUnit.CENTIMETERS;

                default:
                    throw new InvalidUnitException("Invalid Unit Entered");
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("=================================");
            Console.WriteLine("  Quantity Measurement System");
            Console.WriteLine("=================================");
            Console.WriteLine("1. UC1 - Basic Length Comparison");
            Console.WriteLine("2. UC2 - Weight/Mass Comparison (Coming Soon)");
            Console.WriteLine("3. UC3 - Temperature Comparison (Coming Soon)");
            Console.WriteLine("4. UC4 - Advanced Yard Equality Testing");
            Console.WriteLine("5. Exit");
            Console.WriteLine("=================================");
            Console.Write("Enter your choice (1-5): ");
        }

        static void RunBasicLengthComparison()
        {
            Console.WriteLine("\n=== UC1 - Basic Length Comparison ===");
            try
            {
                Console.Write("Enter first value: ");
                string? input1 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input1))
                    throw new Exception("Value cannot be empty");
                double value1 = Convert.ToDouble(input1);

                Console.Write("Enter first unit (INCHES / FEET / YARDS / CENTIMETERS): ");
                string? unitInput1 = Console.ReadLine();
                LengthUnit unit1 = GetUnit(unitInput1 ?? "");

                Console.Write("Enter second value: ");
                string? input2 = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(input2))
                    throw new Exception("Value cannot be empty");
                double value2 = Convert.ToDouble(input2);

                Console.Write("Enter second unit (INCHES / FEET / YARDS / CENTIMETERS): ");
                string? unitInput2 = Console.ReadLine();
                LengthUnit unit2 = GetUnit(unitInput2 ?? "");

                QuantityLength q1 = new QuantityLength(value1, unit1);
                QuantityLength q2 = new QuantityLength(value2, unit2);

                QuantityMeasurementApp.Compare(q1, q2);
            }
            catch (InvalidLengthException e)
            {
                Console.WriteLine("Length Error: " + e.Message);
            }
            catch (InvalidUnitException e)
            {
                Console.WriteLine("Unit Error: " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected Error: " + e.Message);
            }
        }

        static void RunAdvancedYardEqualityTesting()
        {
            Console.WriteLine("\n=== UC4 - Advanced Yard Equality Testing ===");
            Console.WriteLine("This feature tests comprehensive equality across all length units.");
            Console.WriteLine("Running automated tests...");

            // Run the test suite
            var testAssembly = typeof(Program).Assembly;
            // For now, we'll simulate running tests by calling key test scenarios
            RunYardEqualityScenarios();

            Console.WriteLine("Advanced equality testing completed!");
        }

        static void RunYardEqualityScenarios()
        {
            Console.WriteLine("\nTesting Yard Equality Scenarios:");

            // Test cases from the test suite
            QuantityLength yard1 = new QuantityLength(1.0, LengthUnit.YARDS);
            QuantityLength feet3 = new QuantityLength(3.0, LengthUnit.FEET);
            QuantityLength inches36 = new QuantityLength(36.0, LengthUnit.INCHES);

            Console.WriteLine($"1 yard = 3 feet: {yard1.Equals(feet3)}");
            Console.WriteLine($"3 feet = 36 inches: {feet3.Equals(inches36)}");
            Console.WriteLine($"1 yard = 36 inches: {yard1.Equals(inches36)}");

            // Test different values
            QuantityLength yard2 = new QuantityLength(2.0, LengthUnit.YARDS);
            QuantityLength feet6 = new QuantityLength(6.0, LengthUnit.FEET);
            QuantityLength inches72 = new QuantityLength(72.0, LengthUnit.INCHES);

            Console.WriteLine($"2 yards = 6 feet: {yard2.Equals(feet6)}");
            Console.WriteLine($"6 feet = 72 inches: {feet6.Equals(inches72)}");
            Console.WriteLine($"2 yards = 72 inches: {yard2.Equals(inches72)}");

            // Test centimeters
            QuantityLength cm1 = new QuantityLength(1.0, LengthUnit.CENTIMETERS);
            QuantityLength inch_cm = new QuantityLength(0.393701, LengthUnit.INCHES);
            Console.WriteLine($"1 cm ≈ 0.393701 inches: {cm1.Equals(inch_cm)}");
        }

        static void Main()
        {
            bool running = true;

            while (running)
            {
                DisplayMenu();
                string? choice = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(choice))
                {
                    Console.WriteLine("\nInvalid choice. Please select 1-5.\n");
                    continue;
                }

                switch (choice)
                {
                    case "1":
                        RunBasicLengthComparison();
                        break;
                    case "2":
                        Console.WriteLine("\nUC2 - Weight/Mass Comparison is not yet implemented.");
                        Console.WriteLine("This feature will be available in a future update.\n");
                        break;
                    case "3":
                        Console.WriteLine("\nUC3 - Temperature Comparison is not yet implemented.");
                        Console.WriteLine("This feature will be available in a future update.\n");
                        break;
                    case "4":
                        RunAdvancedYardEqualityTesting();
                        break;
                    case "5":
                        Console.WriteLine("\nThank you for using Quantity Measurement System!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Please select 1-5.\n");
                        break;
                }

                if (running)
                {
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }
    }
}