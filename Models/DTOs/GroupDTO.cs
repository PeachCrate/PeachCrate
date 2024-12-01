using Models.Models;

namespace Models.DTOs;

public record struct GroupDTO(
    int? GroupId,
    string Title,
    string? Description,
    string? InvintationCode,
    bool IsUserGroup,
    DateTime CreationDate,
    List<User>? Users = null
    );

