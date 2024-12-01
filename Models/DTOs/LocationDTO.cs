using Models.Models;

namespace Models.DTOs;

public record struct LocationDTO(
    int LocationId,
    string Title,
    string? Description,
    string? Address,
    Group? Group,
    List<Place> Places
    );