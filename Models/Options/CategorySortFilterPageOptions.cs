using Models.Enums;

namespace Models.Options;

public class CategorySortFilterPageOptions
{
    public CategoryOrderBy OrderBy { get; set; }
    public CategoryFilterBy FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public int PageStart { get; set; }
    public int PageNum { get; set; }
}
