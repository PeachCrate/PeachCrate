namespace Models.Models;

public class ShoppingList
{
    public int ShoppingListId { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTime? PlannedShoppingDate { get; set; }
    public int? GroupId { get; set; }
    public Group? Group { get; set; }
    public List<ShoppingProduct>? ShoppingProducts { get; set; }
}
