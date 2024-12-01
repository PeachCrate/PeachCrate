using Models.Models;

namespace Models.DTOs;

public record struct ProductDTO(
    int ProductId,
    int ProductBaseId,
    ProductBase? ProductBase,
    int Quantity,
    double? Price,
    DateTime? PurchaseDate,
    DateTime? ValidUntil,
    int PlaceId,
    Place? Place
    );