namespace Models.Models;

public class Product
{
    public int ProductId { get; set; }
    public int ProductBaseId { get; set; }
    public ProductBase? ProductBase { get; set; }
    public int Quantity { get; set; }
    public double? Price { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public DateTime? ValidUntil { get; set; }
    public int PlaceId { get; set; }
    public Place? Place { get; set; }

}
