// File: InvalidLengthException.cs
// Description: Custom exception class for invalid length inputs
// Purpose: Throws exceptions when negative values or unsupported units are used


using System;

public class InvalidLengthException : Exception
{
    public InvalidLengthException(string message) : base(message)
    {
    }
}