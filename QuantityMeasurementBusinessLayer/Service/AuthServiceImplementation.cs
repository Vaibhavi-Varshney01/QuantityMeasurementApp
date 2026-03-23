using QuantityMeasurementBusinessLayer.DTO;
using QuantityMeasurementBusinessLayer.Helper;
using QuantityMeasurementBusinessLayer.Interface;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository;

namespace QuantityMeasurementBusinessLayer.Service;

public class AuthServiceImpl : IAuthService
{
    private readonly QuantityMeasurementDbContext _context;
    private readonly JwtHelper _jwtHelper;

    public AuthServiceImpl(
        QuantityMeasurementDbContext context,
        JwtHelper jwtHelper)
    {
        _context   = context;
        _jwtHelper = jwtHelper;
    }

    public AuthResponseDTO Register(RegisterDTO dto)
    {
        // Check if username already exists
        bool exists = _context.Users
            .Any(u => u.Username == dto.Username);

        if (exists)
            throw new Exception($"Username '{dto.Username}' already taken.");

        // Step 1 — Generate unique salt for this user
        string salt = PasswordHelper.GenerateSalt();

        // Step 2 — Hash the password with the salt
        string hash = PasswordHelper.HashPassword(dto.Password, salt);

        // Step 3 — Save user with hash and salt (NEVER save plain password)
        var user = new UserEntity
        {
            Username     = dto.Username,
            PasswordHash = hash,
            Salt         = salt,
            Role         = "User"
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // Step 4 — Generate JWT token
        string token = _jwtHelper.GenerateToken(user);

        return new AuthResponseDTO
        {
            Token     = token,
            Username  = user.Username,
            Role      = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }

    public AuthResponseDTO Login(LoginDTO dto)
    {
        // Find user by username
        var user = _context.Users
            .FirstOrDefault(u => u.Username == dto.Username);

        if (user == null)
            throw new Exception("Invalid username or password.");

        // Step 1 — Verify the password against stored hash
        bool isValid = PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash);

        if (!isValid)
            throw new Exception("Invalid username or password.");

        // Step 2 — Generate JWT token
        string token = _jwtHelper.GenerateToken(user);

        return new AuthResponseDTO
        {
            Token     = token,
            Username  = user.Username,
            Role      = user.Role,
            ExpiresAt = DateTime.UtcNow.AddHours(1)
        };
    }
}
 
