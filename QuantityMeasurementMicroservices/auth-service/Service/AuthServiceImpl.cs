using AuthService.DBContext;
using AuthService.Helper;
using AuthService.Interface;
using SharedModels.DTO;
using SharedModels.Entities;

namespace AuthService.Service;

public class AuthServiceImpl : IAuthService
{
    private readonly AuthDbContext _ctx;
    private readonly JwtHelper    _jwt;
    public AuthServiceImpl(AuthDbContext ctx, JwtHelper jwt) { _ctx = ctx; _jwt = jwt; }

    public AuthResponseDTO Register(RegisterDTO dto)
    {
        if (_ctx.Users.Any(u => u.Username == dto.Username))
            throw new Exception($"Username '{dto.Username}' already taken.");

        var salt = PasswordHelper.GenerateSalt();
        var hash = PasswordHelper.HashPassword(dto.Password, salt);
        var user = new UserEntity { Username = dto.Username, PasswordHash = hash, Salt = salt, Role = "User" };
        _ctx.Users.Add(user);
        _ctx.SaveChanges();

        return new AuthResponseDTO { Token = _jwt.GenerateToken(user), Username = user.Username, Role = user.Role, ExpiresAt = DateTime.UtcNow.AddHours(1) };
    }

    public AuthResponseDTO Login(LoginDTO dto)
    {
        var user = _ctx.Users.FirstOrDefault(u => u.Username == dto.Username)
            ?? throw new Exception("Invalid username or password.");
        if (!PasswordHelper.VerifyPassword(dto.Password, user.PasswordHash))
            throw new Exception("Invalid username or password.");
        return new AuthResponseDTO { Token = _jwt.GenerateToken(user), Username = user.Username, Role = user.Role, ExpiresAt = DateTime.UtcNow.AddHours(1) };
    }
}
