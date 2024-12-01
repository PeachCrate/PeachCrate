using System.Text.Json.Serialization;

namespace Models.Models;

public class ProductBase
{
    public int ProductBaseId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public double? Weight { get; set; }
    public int? RunningOutQuantity { get; set; }
    public List<Category>? Categories { get; set; }
    [JsonIgnore]
    public List<Product>? Products { get; set; }
    [JsonIgnore]
    public List<ShoppingProduct>? ShoppingProducts { get; set; }
}
