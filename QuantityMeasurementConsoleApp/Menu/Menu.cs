using System;
using QuantityMeasurementModel.Models;
using QuantityMeasurementRepository;

namespace QuantityMeasurementApp
{
    class Menu : IMenu
    {
        public void ShowMenu()
        {
            QuantityMeasurementService service = new QuantityMeasurementService();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n===== Quantity Measurement Menu (UC1 - UC14) =====");
                Console.WriteLine("1. Compare Lengths (Feet/Inch/Yard/Cm)");
                Console.WriteLine("2. Convert Length Units");
                Console.WriteLine("3. Add Lengths");
                Console.WriteLine("4. Add Lengths with Target Unit");
                Console.WriteLine("5. Subtract Lengths");
                Console.WriteLine("6. Subtract Lengths with Target Unit");
                Console.WriteLine("7. Divide Lengths");
                Console.WriteLine("8. Compare Weight");
                Console.WriteLine("9. Convert Weight Units");
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
                        // 1: Compare Lengths
                        case "1":
                            Console.Write("Enter first value: ");
                            double firstLengthValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit firstLengthUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            Console.Write("Enter second value: ");
                            double secondLengthValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit secondLengthUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var quantityOne = new Quantity<LengthUnit>(firstLengthValue, firstLengthUnit);
                            var quantityTwo = new Quantity<LengthUnit>(secondLengthValue, secondLengthUnit);

                            Console.WriteLine("Equal: " + service.GenericAreEqual<LengthUnit>(quantityOne, quantityTwo));
                            break;

                        // 2: Convert Length Units
                        case "2":
                            Console.Write("Enter value: ");
                            double lengthValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Feet/Inch/Yard/Cm): ");
                            LengthUnit sourceUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit targetUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var convertedQuantity = service.GenericConvert(new Quantity<LengthUnit>(lengthValue, sourceUnit), targetUnit);
                            Console.WriteLine($"Converted: {convertedQuantity.Value} {convertedQuantity.Unit}");
                            break;

                        // 3: Add Lengths
                        case "3":
                            Console.Write("Enter first value: ");
                            double addFirstValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit addFirstUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double addSecondValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit addSecondUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var addResult = service.GenericAdd<LengthUnit>(
                                new Quantity<LengthUnit>(addFirstValue, addFirstUnit),
                                new Quantity<LengthUnit>(addSecondValue, addSecondUnit)
                            );
                            Console.WriteLine($"Sum = {addResult.Value} {addResult.Unit}");
                            break;

                        // 4: Add Lengths with Target Unit
                        case "4":
                            Console.Write("Enter first value: ");
                            double addTargetFirstValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit addTargetFirstUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double addTargetSecondValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit addTargetSecondUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit targetLengthUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var addWithTargetResult = service.GenericAdd<LengthUnit>(
                                new Quantity<LengthUnit>(addTargetFirstValue, addTargetFirstUnit),
                                new Quantity<LengthUnit>(addTargetSecondValue, addTargetSecondUnit),
                                targetLengthUnit
                            );
                            Console.WriteLine($"Sum = {addWithTargetResult.Value} {addWithTargetResult.Unit}");
                            break;

                        // 5: Subtract Lengths
                        case "5":
                            Console.Write("Enter first value: ");
                            double subtractFirstValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit subtractFirstUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double subtractSecondValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit subtractSecondUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var subtractResult = service.GenericSubtract<LengthUnit>(
                                new Quantity<LengthUnit>(subtractFirstValue, subtractFirstUnit),
                                new Quantity<LengthUnit>(subtractSecondValue, subtractSecondUnit)
                            );
                            Console.WriteLine($"Subtraction Result = {subtractResult.Value} {subtractResult.Unit}");
                            break;

                        // 6: Subtract Lengths with Target Unit
                        case "6":
                            Console.Write("Enter first value: ");
                            double subtractTargetFirstValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit subtractTargetFirstUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double subtractTargetSecondValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit subtractTargetSecondUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit: ");
                            LengthUnit targetSubtractUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var subtractTargetResult = service.GenericSubtract<LengthUnit>(
                                new Quantity<LengthUnit>(subtractTargetFirstValue, subtractTargetFirstUnit),
                                new Quantity<LengthUnit>(subtractTargetSecondValue, subtractTargetSecondUnit),
                                targetSubtractUnit
                            );
                            Console.WriteLine($"Subtraction Result = {subtractTargetResult.Value} {subtractTargetResult.Unit}");
                            break;

                        // 7: Divide Lengths
                        case "7":
                            Console.Write("Enter first value: ");
                            double divideFirstValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit: ");
                            LengthUnit divideFirstUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter second value: ");
                            double divideSecondValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit: ");
                            LengthUnit divideSecondUnit = Enum.Parse<LengthUnit>(Console.ReadLine()!, true);

                            var divideResult = service.GenericDivide<LengthUnit>(
                                new Quantity<LengthUnit>(divideFirstValue, divideFirstUnit),
                                new Quantity<LengthUnit>(divideSecondValue, divideSecondUnit)
                            );
                            Console.WriteLine($"Division Result = {divideResult}");
                            break;

                        // 8-12: Weight operations (same pattern)
                        case "8":
                        case "9":
                        case "10":
                        case "11":
                        case "12":
                            Console.WriteLine("Weight operations can be added using same generic methods with WeightUnit.");
                            break;

                        case "13":
                            Console.Write("Enter first temperature value: ");
                            double tempFirstValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter first unit (Celsius/Fahrenheit/Kelvin): ");
                            TemperatureUnit tempFirstUnit = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);

                            Console.Write("Enter second temperature value: ");
                            double tempSecondValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second unit (Celsius/Fahrenheit/Kelvin): ");
                            TemperatureUnit tempSecondUnit = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);

                            var tempQuantityOne = new Quantity<TemperatureUnit>(tempFirstValue, tempFirstUnit);
                            var tempQuantityTwo = new Quantity<TemperatureUnit>(tempSecondValue, tempSecondUnit);
                            Console.WriteLine("Equal: " + service.GenericAreEqual<TemperatureUnit>(tempQuantityOne, tempQuantityTwo));
                            break;

                        case "14":
                            Console.Write("Enter temperature value: ");
                            double tempValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter source unit (Celsius/Fahrenheit/Kelvin): ");
                            TemperatureUnit tempSourceUnit = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);
                            Console.Write("Enter target unit (Celsius/Fahrenheit/Kelvin): ");
                            TemperatureUnit tempTargetUnit = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);

                            var tempConverted = service.GenericConvert(new Quantity<TemperatureUnit>(tempValue, tempSourceUnit), tempTargetUnit);
                            Console.WriteLine($"Converted: {tempConverted.Value} {tempConverted.Unit}");
                            break;

                        case "15":
                            Console.Write("Enter first temperature value: ");
                            double tempArithmeticFirstValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter second temperature value: ");
                            double tempArithmeticSecondValue = double.Parse(Console.ReadLine()!);
                            Console.Write("Enter unit (Celsius/Fahrenheit/Kelvin): ");
                            TemperatureUnit tempArithmeticUnit = Enum.Parse<TemperatureUnit>(Console.ReadLine()!, true);

                            var tempArithmeticQuantityOne = new Quantity<TemperatureUnit>(tempArithmeticFirstValue, tempArithmeticUnit);
                            var tempArithmeticQuantityTwo = new Quantity<TemperatureUnit>(tempArithmeticSecondValue, tempArithmeticUnit);

                            try
                            {
                                service.GenericAdd(tempArithmeticQuantityOne, tempArithmeticQuantityTwo);
                            }
                            catch (NotSupportedException nse)
                            {
                                Console.WriteLine("Add: " + nse.Message);
                            }

                            try
                            {
                                service.GenericSubtract(tempArithmeticQuantityOne, tempArithmeticQuantityTwo);
                            }
                            catch (NotSupportedException nse)
                            {
                                Console.WriteLine("Subtract: " + nse.Message);
                            }

                            try
                            {
                                service.GenericDivide(tempArithmeticQuantityOne, tempArithmeticQuantityTwo);
                            }
                            catch (NotSupportedException nse)
                            {
                                Console.WriteLine("Divide: " + nse.Message);
                            }
                            break;

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
                }
            }
        }
    }
}
