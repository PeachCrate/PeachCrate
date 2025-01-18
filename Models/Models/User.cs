using System.ComponentModel.DataAnnotations;

namespace Models.Models;

public class User
{
    public int? UserId { get; set; }
    public string? ClerkId { get; set; }
    [MaxLength(20)]
    [MinLength(5)]
    public string Login { get; set; }
    [DoNotExport]
    public string? PasswordHash { get; set; }
    [DoNotExport]
    public string? PasswordSalt { get; set; }
    [Phone]
    public string? PhoneNumber { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<Group>? Groups { get; set; }
    public RefreshToken? RefreshToken { get; set; }
}