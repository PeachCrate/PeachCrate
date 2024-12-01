using Models.Models;

namespace Models.DTOs;

public record struct ShoppingListDTO(
    int ShoppingListId,
    string Name,
    string? Description,
    DateTime? PlannedShoppingDate,
    Group? Group,
    List<ShoppingProduct> ShoppingProducts
    );