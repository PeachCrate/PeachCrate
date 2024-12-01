using Models.Models;

namespace Models.DTOs;

public record struct CategoryDTO(
    int CategoryId,
    string Title,
    string? Description,
    List<ProductBase>? ProductBases
    );