using Models.Models;

namespace Models.DTOs;

public record struct ShoppingProductDTO(
    int ShoppingProductId,
    int ProductBaseId,
    ProductBase? ProductBase,
    int? Quantity,
    int? ShoppingListId,
    ShoppingList? ShoppingList
    );