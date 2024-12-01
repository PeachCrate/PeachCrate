namespace Models.Models;

public class Place
{
    public int PlaceId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public int LocationId { get; set; }
    public Location? Location { get; set; }
    public List<Product>? Products { get; set; }
}
