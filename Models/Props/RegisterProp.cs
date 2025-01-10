using System.ComponentModel.DataAnnotations;

namespace Models.Props;

public record struct RegisterProp(
    [MaxLength(20)] [MinLength(5)] string Login,
    [EmailAddress] string Email,
    string Password,
    string ClerkId);