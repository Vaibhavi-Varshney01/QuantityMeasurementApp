namespace AuthService.Helper;

public static class PasswordHelper
{
    public static string GenerateSalt()   => BCrypt.Net.BCrypt.GenerateSalt(12);
    public static string HashPassword(string plain, string salt) => BCrypt.Net.BCrypt.HashPassword(plain, salt);
    public static bool   VerifyPassword(string plain, string hash) => BCrypt.Net.BCrypt.Verify(plain, hash);
}
