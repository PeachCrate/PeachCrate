namespace Models.Enums;

public enum ShoppingProductOrderBy
{
    // product options
    ByProductIdDESC, ByQuantityDESC,
    ByProductIdASC, ByQuantityASC,
    // productBase options
    ByProductBaseIdDESC, ByNameDESC, ByWeightDESC,
    ByProductBaseIdASC, ByNameASC, ByWeightASC,

}

public enum ShoppingProductFilterBy
{
    NoFilter,
    ByQuantitySmallerThan, ByQuantityBiggerThan,
    ByName, ByWeightSmallerThan, ByWeightBiggerThan,
}