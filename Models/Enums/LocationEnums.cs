namespace Models.Enums;

public enum LocationOrderBy
{
    ByLocationIdASC, ByTitleASC, ByAddressASC, ByLocationIdDESC, ByTitleDESC, ByAddressDESC
}
public enum LocationFilterBy
{
    NoFilter, ByTitle, ByAddress
}