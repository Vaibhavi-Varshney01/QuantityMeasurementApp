using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementModel.Entities;

namespace QuantityMeasurementBusinessLayer.Helper;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
        => _configuration = configuration;

    public string GenerateToken(UserEntity user)
    {
        // Read secret key from appsettings.json
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        var credentials = new SigningCredentials(
            key, SecurityAlgorithms.HmacSha256);

        // Claims are the data stored inside the token
        var claims = new[]
        {
            new Claim(ClaimTypes.Name,           user.Username),
            new Claim(ClaimTypes.Role,           user.Role),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        var expiry = DateTime.UtcNow.AddHours(
            double.Parse(_configuration["Jwt:ExpiryHours"]!));

        var token = new JwtSecurityToken(
            issuer:             _configuration["Jwt:Issuer"],
            audience:           _configuration["Jwt:Audience"],
            claims:             claims,
            expires:            expiry,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
 
