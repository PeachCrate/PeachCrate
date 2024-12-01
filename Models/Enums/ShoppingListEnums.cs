namespace Models.Enums;

public enum ShoppingListOrderBy
{
    ByShoppingListIdASC, ByNameASC, ByPlannedShoppingDateASC,
    ByShoppingListIdDESC, ByNameDESC, ByPlannedShoppingDateDESC
}
public enum ShoppingListFilterBy
{
    NoFilter, ByName, ByPlannedShoppingDateBefore, ByPlannedShoppingDateAfter
}