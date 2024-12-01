using Models.Enums;

namespace Models.Options;

public class GroupSortFilterPageOptions
{
    public GroupOrderBy OrderBy { get; set; }
    public GroupFilterBy FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public int PageStart { get; set; }
    public int PageNum { get; set; }
}
