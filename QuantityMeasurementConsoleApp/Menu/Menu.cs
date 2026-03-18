using System;
using QuantityMeasurementModel.Models;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository;

namespace QuantityMeasurementApp
{
    class Menu : IMenu
    {
        private readonly IQuantityMeasurementRepository _repo;

        public Menu(IQuantityMeasurementRepository repo)
        {
            _repo = repo;
        }

        public void ShowMenu()
        {
            QuantityMeasurementService service = new QuantityMeasurementService();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Quantity Measurement Menu (UC1 - UC14) =====");
                Console.WriteLine("1.  Compare Lengths (Feet/Inch/Yard/Cm)");
                Console.WriteLine("2.  Convert Length Units");
                Console.WriteLine("3.  Add Lengths");
                Console.WriteLine("4.  Add Lengths with Target Unit");
                Console.WriteLine("5.  Subtract Lengths");
                Console.WriteLine("6.  Subtract Lengths with Target Unit");
                Console.WriteLine("7.  Divide Lengths");
                Console.WriteLine("8.  Compare Weight");
                Console.WriteLine("9.  Convert Weight Units");
                Console.WriteLine("10. Add Weight");
                Console.WriteLine("11. Subtract Weight");
                Console.WriteLine("12. Divide Weight");
                Console.WriteLine("13. Compare Temperature");
                Console.WriteLine("14. Convert Temperature");
                Console.WriteLine("15. Demo Unsupported Temperature Arithmetic");
                Console.WriteLine("16. Exit");

                Console.Write("Enter choice: ");
                string choice = Console.ReadLine()!;

                try
                {
                    switch (choice)
                    {
                        // ── 1: Compare Lengths ────────────────────────────────
                        case "1":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var q1 = new Quantity<LengthUnit>(v1, u1);
                            var q2 = new Quantity<LengthUnit>(v2, u2);
                            bool result = service.GenericAreEqual<LengthUnit>(q1, q2);

                            Console.WriteLine("Equal: " + result);

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "COMPARE", result.ToString(), "Length"));
                            break;
                        }

                        // ── 2: Convert Length ─────────────────────────────────
                        case "2":
                        {
                            Console.Write("Enter value: ");
                            double v = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit from = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit to = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var converted = service.GenericConvert(
                                new Quantity<LengthUnit>(v, from), to);

                            Console.WriteLine($"Converted: {converted.Value} {converted.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                operand1: $"{v} {from}",
                                operationType: "CONVERT",
                                result: $"{converted.Value} {converted.Unit}",
                                measurementType: "Length"));
                            break;
                        }

                        // ── 3: Add Lengths ────────────────────────────────────
                        case "3":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var result = service.GenericAdd<LengthUnit>(
                                new Quantity<LengthUnit>(v1, u1),
                                new Quantity<LengthUnit>(v2, u2));

                            Console.WriteLine($"Sum = {result.Value} {result.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "ADD", $"{result.Value} {result.Unit}", "Length"));
                            break;
                        }

                        // ── 4: Add Lengths with Target Unit ───────────────────
                        case "4":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var result = service.GenericAdd<LengthUnit>(
                                new Quantity<LengthUnit>(v1, u1),
                                new Quantity<LengthUnit>(v2, u2),
                                target);

                            Console.WriteLine($"Sum = {result.Value} {result.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "ADD", $"{result.Value} {result.Unit}", "Length"));
                            break;
                        }

                        // ── 5: Subtract Lengths ───────────────────────────────
                        case "5":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var result = service.GenericSubtract<LengthUnit>(
                                new Quantity<LengthUnit>(v1, u1),
                                new Quantity<LengthUnit>(v2, u2));

                            Console.WriteLine($"Subtraction Result = {result.Value} {result.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "SUBTRACT", $"{result.Value} {result.Unit}", "Length"));
                            break;
                        }

                        // ── 6: Subtract Lengths with Target Unit ──────────────
                        case "6":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit target = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var result = service.GenericSubtract<LengthUnit>(
                                new Quantity<LengthUnit>(v1, u1),
                                new Quantity<LengthUnit>(v2, u2),
                                target);

                            Console.WriteLine($"Subtraction Result = {result.Value} {result.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "SUBTRACT", $"{result.Value} {result.Unit}", "Length"));
                            break;
                        }

                        // ── 7: Divide Lengths ─────────────────────────────────
                        case "7":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit u1 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit u2 = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            double result = service.GenericDivide<LengthUnit>(
                                new Quantity<LengthUnit>(v1, u1),
                                new Quantity<LengthUnit>(v2, u2));

                            Console.WriteLine($"Division Result = {result}");

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "DIVIDE", result.ToString(), "Length"));
                            break;
                        }

                        // ── 8: Compare Weight ─────────────────────────────────
                        case "8":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Kilogram/Gram/Pound/Tonne): ");
                            WeightUnit u1 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            WeightUnit u2 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);

                            bool result = service.GenericAreEqual<WeightUnit>(
                                new Quantity<WeightUnit>(v1, u1),
                                new Quantity<WeightUnit>(v2, u2));

                            Console.WriteLine("Equal: " + result);

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "COMPARE", result.ToString(), "Weight"));
                            break;
                        }

                        // ── 9: Convert Weight ─────────────────────────────────
                        case "9":
                        {
                            Console.Write("Enter value: ");
                            double v = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Kilogram/Gram/Pound/Tonne): ");
                            WeightUnit from = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            WeightUnit to = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);

                            var converted = service.GenericConvert(
                                new Quantity<WeightUnit>(v, from), to);

                            Console.WriteLine($"Converted: {converted.Value} {converted.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                operand1: $"{v} {from}",
                                operationType: "CONVERT",
                                result: $"{converted.Value} {converted.Unit}",
                                measurementType: "Weight"));
                            break;
                        }

                        // ── 10: Add Weight ────────────────────────────────────
                        case "10":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Kilogram/Gram/Pound/Tonne): ");
                            WeightUnit u1 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            WeightUnit u2 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);

                            var result = service.GenericAdd<WeightUnit>(
                                new Quantity<WeightUnit>(v1, u1),
                                new Quantity<WeightUnit>(v2, u2));

                            Console.WriteLine($"Sum = {result.Value} {result.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "ADD", $"{result.Value} {result.Unit}", "Weight"));
                            break;
                        }

                        // ── 11: Subtract Weight ───────────────────────────────
                        case "11":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            WeightUnit u1 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            WeightUnit u2 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);

                            var result = service.GenericSubtract<WeightUnit>(
                                new Quantity<WeightUnit>(v1, u1),
                                new Quantity<WeightUnit>(v2, u2));

                            Console.WriteLine($"Subtraction Result = {result.Value} {result.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "SUBTRACT", $"{result.Value} {result.Unit}", "Weight"));
                            break;
                        }

                        // ── 12: Divide Weight ─────────────────────────────────
                        case "12":
                        {
                            Console.Write("Enter first value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            WeightUnit u1 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            WeightUnit u2 = Enum.Parse<WeightUnit>(Console.ReadLine()!, true);

                            double result = service.GenericDivide<WeightUnit>(
                                new Quantity<WeightUnit>(v1, u1),
                                new Quantity<WeightUnit>(v2, u2));

                            Console.WriteLine($"Division Result = {result}");

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "DIVIDE", result.ToString(), "Weight"));
                            break;
                        }

                        // ── 13: Compare Temperature ───────────────────────────
                        case "13":
                        {
                            Console.Write("Enter first temperature value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Celsius/Fahrenheit/Kelvin): ");
                            TemperatureUnit u1 = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second temperature value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            TemperatureUnit u2 = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);

                            bool result = service.GenericAreEqual<TemperatureUnit>(
                                new Quantity<TemperatureUnit>(v1, u1),
                                new Quantity<TemperatureUnit>(v2, u2));

                            Console.WriteLine("Equal: " + result);

                            _repo.Save(new QuantityMeasurementEntity(
                                $"{v1} {u1}", $"{v2} {u2}",
                                "COMPARE", result.ToString(), "Temperature"));
                            break;
                        }

                        // ── 14: Convert Temperature ───────────────────────────
                        case "14":
                        {
                            Console.Write("Enter temperature value: ");
                            double v = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter source unit (Celsius/Fahrenheit/Kelvin): ");
                            TemperatureUnit from = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            TemperatureUnit to = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);

                            var converted = service.GenericConvert(
                                new Quantity<TemperatureUnit>(v, from), to);

                            Console.WriteLine($"Converted: {converted.Value} {converted.Unit}");

                            _repo.Save(new QuantityMeasurementEntity(
                                operand1: $"{v} {from}",
                                operationType: "CONVERT",
                                result: $"{converted.Value} {converted.Unit}",
                                measurementType: "Temperature"));
                            break;
                        }

                        // ── 15: Demo Unsupported Temperature Arithmetic ────────
                        case "15":
                        {
                            Console.Write("Enter first temperature value: ");
                            double v1 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second temperature value: ");
                            double v2 = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Celsius/Fahrenheit/Kelvin): ");
                            TemperatureUnit u = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);

                            var tq1 = new Quantity<TemperatureUnit>(v1, u);
                            var tq2 = new Quantity<TemperatureUnit>(v2, u);

                            foreach (string op in new[] { "ADD", "SUBTRACT", "DIVIDE" })
                            {
                                try
                                {
                                    if (op == "ADD")    service.GenericAdd(tq1, tq2);
                                    if (op == "SUBTRACT") service.GenericSubtract(tq1, tq2);
                                    if (op == "DIVIDE") service.GenericDivide(tq1, tq2);
                                }
                                catch (NotSupportedException nse)
                                {
                                    Console.WriteLine($"{op}: {nse.Message}");
                                    _repo.Save(new QuantityMeasurementEntity(nse.Message));
                                }
                            }
                            break;
                        }

                        case "16":
                            exit = true;
                            Console.WriteLine("Exiting program...");
                            break;

                        default:
                            Console.WriteLine("Invalid choice!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    _repo.Save(new QuantityMeasurementEntity(ex.Message));
                }
            }
        }
    }
}
