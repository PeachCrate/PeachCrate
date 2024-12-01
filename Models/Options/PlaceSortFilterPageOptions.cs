using Models.Enums;

namespace Models.Options;

public class PlaceSortFilterPageOptions
{
    public PlaceOrderBy OrderBy { get; set; }
    public PlaceFilterBy FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public int PageStart { get; set; }
    public int PageNum { get; set; }
}
