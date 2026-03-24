using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementModel.DTO;

public class AuthResponseDTO
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Role { get; set; } = "User";

    public DateTime ExpiresAt { get; set; }
}
 
