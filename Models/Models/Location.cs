namespace Models.Models;

public class Location
{
    public int LocationId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Address { get; set; }
    public int GroupId { get; set; }
    public Group? Group { get; set; }
    public List<Place>? Places { get; set; }
}
