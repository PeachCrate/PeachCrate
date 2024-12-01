using Models.Enums;

namespace Models.Options;

public class LocationSortFilterPageOptions
{
    public LocationOrderBy OrderBy { get; set; }
    public LocationFilterBy FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public int PageStart { get; set; }
    public int PageNum { get; set; }
}