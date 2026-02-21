namespace feet_measurement_equality{
    public class QuantityMeasurementApp{
        public bool Comparefeet(int valueOne, int valueTwo){
            Feet feetValueOne = new Feet(valueOne);
            Feet feetValuetwo = new Feet(valueTwo);

            return feetValueOne.Equals(feetValuetwo);
        }
    }
}