using Models.Enums;

namespace Models.Options;

public class ProductSortFilterPageOptions
{
    public ProductOrderBy OrderBy { get; set; }
    public ProductFilterBy FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public int PageStart { get; set; }
    public int PageNum { get; set; }
}
