using Models.Models;

namespace Models.DTOs;

public record struct PlaceDTO(
    int PlaceId,
    string Name,
    string? Description,
    Location? Location,
    List<Product> Products
    );