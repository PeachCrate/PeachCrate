namespace Models.Enums;

public enum ProductBaseOrderBy
{
    ByProductBaseIdDESC, ByNameDESC, ByWeightDESC,
    ByProductBaseIdASC, ByNameASC, ByWeightASC,
}

public enum ProductBaseFilterBy
{
    NoFilter, ByName, ByWeightSmallerThan, ByWeightBiggerThan
}