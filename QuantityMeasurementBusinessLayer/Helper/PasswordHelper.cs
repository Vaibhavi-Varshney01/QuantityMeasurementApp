namespace QuantityMeasurementBusinessLayer.Helper;

/// <summary>
/// Provides helpers for generating and validating hashed passwords.
/// </summary>
public static class PasswordHelper
{
    /// <summary>
    /// Generate a unique salt value that can be stored for each user.
    /// </summary>
    /// <remarks>
    /// The work factor is set to 12; higher values increase security but slow operations.
    /// </remarks>
    public static string GenerateSalt()
    {
        return BCrypt.Net.BCrypt.GenerateSalt(12);
    }

    /// <summary>
    /// Hash a plain password together with its salt and produce a stored hash.
    /// </summary>
    public static string HashPassword(string plainPassword, string salt)
    {
        return BCrypt.Net.BCrypt.HashPassword(plainPassword, salt);
    }

    /// <summary>
    /// Confirm that a plain password matches the stored hash during login.
    /// </summary>
    public static bool VerifyPassword(string plainPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, storedHash);
    }
}
