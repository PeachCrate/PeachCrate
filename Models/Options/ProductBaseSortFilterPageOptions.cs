using Models.Enums;

namespace Models.Options;
public class ProductBaseSortFilterPageOptions
{
    public ProductBaseOrderBy OrderBy { get; set; }
    public ProductBaseFilterBy FilterBy { get; set; }
    public string? FilterValue { get; set; }
    public int PageStart { get; set; }
    public int PageNum { get; set; }
}