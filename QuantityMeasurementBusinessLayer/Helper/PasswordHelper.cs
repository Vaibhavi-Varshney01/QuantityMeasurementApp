namespace QuantityMeasurementBusinessLayer.Helper;

public static class PasswordHelper
{
    // Step 1 — Generate a unique salt for each user
    public static string GenerateSalt()
    {
        // BCrypt generates a salt internally but we also store it explicitly
        // so you can see it separately
        return BCrypt.Net.BCrypt.GenerateSalt(12);
        // 12 is the work factor — higher = more secure but slower
    }

    // Step 2 — Hash the password using the salt
    public static string HashPassword(string plainPassword, string salt)
    {
        // BCrypt combines the password + salt and produces a hash
        // The hash is always different even for the same password
        // because each user has a unique salt
        return BCrypt.Net.BCrypt.HashPassword(plainPassword, salt);
    }

    // Step 3 — Verify password at login
    public static bool VerifyPassword(string plainPassword, string storedHash)
    {
        // BCrypt extracts the salt from the stored hash automatically
        // and re-hashes the plain password to compare
        return BCrypt.Net.BCrypt.Verify(plainPassword, storedHash);
    }
}
 
