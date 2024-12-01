namespace Models.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public List<ProductBase>? ProductBases { get; set; }
}
