using Models.Models;

namespace Models.DTOs;

public record struct UserDTO(
    int? UserId,
    string Login,
    string Password,
    string? PasswordHash,
    string? PasswordSalt,
    string? PhoneNumber,
    string? Email,
    DateTime RegistrationDate,
    List<Group>? Groups
    );

