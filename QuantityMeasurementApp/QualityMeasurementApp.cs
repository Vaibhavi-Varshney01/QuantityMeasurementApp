using System;

namespace yard_equality
{
    public class QuantityMeasurementApp
    {
        public static void Compare(QuantityLength q1, QuantityLength q2)
        {
            bool result = q1.Equals(q2);

            Console.WriteLine($"Input: {q1} and {q2}");
            Console.WriteLine($"Output: Equal ({result})");
            Console.WriteLine();
        }
    }
}