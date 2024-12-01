using Models.Models;

namespace Models.DTOs;

public record struct ProductBaseDTO(
    int ProductBaseId,
    string Name,
    string? Description,
    double? Weight,
    int? RunningOutQuantity,
    List<Category>? Categories
    );