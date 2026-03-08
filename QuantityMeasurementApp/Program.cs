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
                Console.WriteLine("\n===== Quantity Measurement Menu =====");
                Console.WriteLine("1. Compare Feet (UC1)");
                Console.WriteLine("2. Compare Inches (UC2)");
                Console.WriteLine("3. Compare Lengths (UC3 + UC4)");
                Console.WriteLine("4. Convert Units (UC5)");
                Console.WriteLine("5. Add Lengths (UC6)");
                Console.WriteLine("6. Add Lengths with Target Unit (UC7)");
                Console.WriteLine("7. LengthUnit Conversion Demo (UC8)");
                Console.WriteLine("8. Exit");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter first feet value: ");
                        double f1 = double.Parse(Console.ReadLine());

                        Console.Write("Enter second feet value: ");
                        double f2 = double.Parse(Console.ReadLine());

                        Console.WriteLine("Equal: " +
                            service.AreFeetEqual(new Feet(f1), new Feet(f2)));
                        break;

                    case "2":
                        Console.Write("Enter first inches value: ");
                        double i1 = double.Parse(Console.ReadLine());

                        Console.Write("Enter second inches value: ");
                        double i2 = double.Parse(Console.ReadLine());

                        Console.WriteLine("Equal: " +
                            service.AreInchesEqual(new Inches(i1), new Inches(i2)));
                        break;

                    case "3":
                        Console.Write("Enter first value: ");
                        double v1 = double.Parse(Console.ReadLine());

                        Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                        LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        Console.Write("Enter second value: ");
                        double v2 = double.Parse(Console.ReadLine());

                        Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                        LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        Console.WriteLine("Equal: " +
                            service.AreEqual(
                                new QuantityLength(v1, u1),
                                new QuantityLength(v2, u2)));
                        break;

                    case "4":
                        Console.Write("Enter value: ");
                        double value = double.Parse(Console.ReadLine());

                        Console.Write("Enter source unit: ");
                        LengthUnit source = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        Console.Write("Enter target unit: ");
                        LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        double converted = service.Convert(value, source, target);

                        Console.WriteLine($"Converted Value = {converted} {target}");
                        break;

                    case "5":
                        Console.Write("Enter first value: ");
                        double a1 = double.Parse(Console.ReadLine());

                        Console.Write("Enter first unit: ");
                        LengthUnit au1 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        Console.Write("Enter second value: ");
                        double a2 = double.Parse(Console.ReadLine());

                        Console.Write("Enter second unit: ");
                        LengthUnit au2 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        QuantityLength sum = service.Add(
                            new QuantityLength(a1, au1),
                            new QuantityLength(a2, au2));

                        Console.WriteLine($"Sum = {sum.Value} {sum.Unit}");
                        break;

                    case "6":
                        Console.Write("Enter first value: ");
                        double b1 = double.Parse(Console.ReadLine());

                        Console.Write("Enter first unit: ");
                        LengthUnit bu1 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        Console.Write("Enter second value: ");
                        double b2 = double.Parse(Console.ReadLine());

                        Console.Write("Enter second unit: ");
                        LengthUnit bu2 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        Console.Write("Enter target unit: ");
                        LengthUnit targetUnit = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        QuantityLength result = service.Add(
                            new QuantityLength(b1, bu1),
                            new QuantityLength(b2, bu2),
                            targetUnit);

                        Console.WriteLine($"Sum = {result.Value} {result.Unit}");
                        break;

                    case "7":
                        // UC8 demonstration
                        Console.Write("Enter value: ");
                        double uc8Val = double.Parse(Console.ReadLine());

                        Console.Write("Enter unit (Feet/Inch/Yard/Cm): ");
                        LengthUnit uc8Unit = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        double baseValue = uc8Unit.ConvertToBaseUnit(uc8Val);

                        Console.WriteLine($"{uc8Val} {uc8Unit} = {baseValue} Feet (Base Unit)");

                        Console.Write("Convert base to unit: ");
                        LengthUnit targetUC8 = Enum.Parse<LengthUnit>(Console.ReadLine(), true);

                        double finalValue = targetUC8.ConvertFromBaseUnit(baseValue);

                        Console.WriteLine($"{baseValue} Feet = {finalValue} {targetUC8}");
                        break;

                    case "8":
                        exit = true;
                        Console.WriteLine("Exiting program...");
                        break;

                    default:
                        Console.WriteLine("Invalid choice!");
                        break;
                }
            }
        }
    }
}