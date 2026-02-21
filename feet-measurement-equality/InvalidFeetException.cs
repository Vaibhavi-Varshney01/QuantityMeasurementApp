using System;
namespace feet_measurement_equality{
    public class InvalidFeetException : Exception {
        public InvalidFeetException(string message) : base(message){
            
        }
    }
}