namespace Models.Models;

public class ShoppingProduct
{
    public int ShoppingProductId { get; set; }
    public int ProductBaseId { get; set; }
    public ProductBase ProductBase { get; set; }
    public int? Quantity { get; set; }
    public int? ShoppingListId { get; set; }
    public ShoppingList? ShoppingList { get; set; }
}
