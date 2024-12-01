using Models.Enums;

namespace Models.Options;

public class UserSortFilterPageOptions
{
    public UserOrderBy OrderBy { get; set; }
    public UserFilterBy FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public int PageStart { get; set; }
    public int PageNum { get; set; }
}