namespace Models.Models;

public class Group
{
    public int? GroupId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? InvintationCode { get; set; }
    public bool IsUserGroup { get; set; }
    public DateTime CreationDate { get; set; }
    public List<User>? Users { get; set; }
    public List<Location>? Locations { get; set; }
    public List<ShoppingList>? ShoppingLists { get; set; }
}
