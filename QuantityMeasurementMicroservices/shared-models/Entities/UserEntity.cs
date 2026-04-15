using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedModels.Entities;

[Table("users")]
public class UserEntity
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    [Required][MaxLength(100)] public string Username { get; set; } = string.Empty;
    [Required][MaxLength(255)] public string PasswordHash { get; set; } = string.Empty;
    [Required][MaxLength(255)] public string Salt { get; set; } = string.Empty;
    [MaxLength(50)] public string Role { get; set; } = "User";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
