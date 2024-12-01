namespace Models.Enums;

public enum ProductOrderBy
{
    // product options
    ByProductIdDESC, ByQuantityDESC, ByPriceDESC,
    ByProductIdASC, ByQuantityASC, ByPriceASC,
    // productBase options
    ByProductBaseIdDESC, ByNameDESC, ByWeightDESC,
    ByProductBaseIdASC, ByNameASC, ByWeightASC,
    // place options
    ByPlaceIdASC, ByPlaceNameASC,
    ByPlaceIdDESC, ByPlaceNameDESC
}

public enum ProductFilterBy
{
    // product options
    NoFilter,
    ByQuantitySmallerThan, ByQuantityBiggerThan,
    ByPriceSmallerThan, ByPriceBiggerThan,
    ByPurchaseDateBefore, ByPurchaseDateAfter,
    ByValidUntilBefore, ByValidUntilAfter,
    // productBase options
    ByName, ByWeightSmallerThan, ByWeightBiggerThan,
    // place options
    ByPlaceName
}