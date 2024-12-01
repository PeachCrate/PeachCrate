using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class ShoppingListLinqExtensions
{
    public static IQueryable<ShoppingListDTO> MapShoppingListsToDTO(this IQueryable<ShoppingList> shoppingLists)
    {
        return shoppingLists.Select(sl => new ShoppingListDTO
        {
            ShoppingListId = sl.ShoppingListId,
            Name = sl.Name,
            Description = sl.Description,
            PlannedShoppingDate = sl.PlannedShoppingDate,
            Group = sl.Group,
            ShoppingProducts = sl.ShoppingProducts
        });
    }
    public static IQueryable<ShoppingList> MapDTOToShoppingLists(this IQueryable<ShoppingListDTO> shoppingLists)
    {
        return shoppingLists.Select(sl => new ShoppingList
        {
            ShoppingListId = sl.ShoppingListId,
            Name = sl.Name,
            Description = sl.Description,
            PlannedShoppingDate = sl.PlannedShoppingDate,
            Group = sl.Group,
            ShoppingProducts = sl.ShoppingProducts
        });
    }

    public static ShoppingList RemoveRecursion(this ShoppingList shoppingList)
    {
        if (shoppingList.Group != null)
        {
            if (shoppingList.Group.ShoppingLists != null)
                shoppingList.Group.Locations = null;

            if (shoppingList.Group.Users != null)
                shoppingList.Group.Users = null;

            if (shoppingList.Group.Locations != null)
                shoppingList.Group.Locations = null;

            if (shoppingList.Group.ShoppingLists != null)
                shoppingList.Group.ShoppingLists = null;
        }
        if (shoppingList.ShoppingProducts != null)
        {
            shoppingList.ShoppingProducts.ForEach(sp =>
            {
                sp.ShoppingList = null;
                if (sp.ProductBase != null)
                    sp.ProductBase.ShoppingProducts = null;
            });
        }
        return shoppingList;
    }
    public static List<ShoppingList> RemoveRecursion(this List<ShoppingList> shoppingList)
    {
        shoppingList.ForEach(shoppingList => shoppingList.RemoveRecursion());
        return shoppingList;
    }
    public static IQueryable<ShoppingListDTO> OrderShoppingListsBy(
        this IQueryable<ShoppingListDTO> shoppingLists,
        ShoppingListOrderBy orderByOptions
        )
    {
        switch (orderByOptions)
        {
            case ShoppingListOrderBy.ByShoppingListIdDESC:
                return shoppingLists.OrderByDescending(sl => sl.ShoppingListId);
            case ShoppingListOrderBy.ByNameDESC:
                return shoppingLists.OrderByDescending(sl => sl.Name);
            case ShoppingListOrderBy.ByPlannedShoppingDateDESC:
                return shoppingLists.OrderByDescending(sl => sl.PlannedShoppingDate);

            case ShoppingListOrderBy.ByShoppingListIdASC:
                return shoppingLists.OrderBy(sl => sl.ShoppingListId);
            case ShoppingListOrderBy.ByNameASC:
                return shoppingLists.OrderBy(sl => sl.Name);
            case ShoppingListOrderBy.ByPlannedShoppingDateASC:
                return shoppingLists.OrderBy(sl => sl.PlannedShoppingDate);
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }
    public static IQueryable<ShoppingListDTO> FilterShoppingListsBy(
        this IQueryable<ShoppingListDTO> ShoppingLists,
        ShoppingListFilterBy filterBy,
        string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
            return ShoppingLists;

        switch (filterBy)
        {
            case ShoppingListFilterBy.NoFilter:
                return ShoppingLists;
            case ShoppingListFilterBy.ByName:
                return ShoppingLists.Where(x => x.Name.Contains(filterValue));
            case ShoppingListFilterBy.ByPlannedShoppingDateBefore:
                return ShoppingLists.Where(x => x.PlannedShoppingDate < Convert.ToDateTime(filterValue));
            case ShoppingListFilterBy.ByPlannedShoppingDateAfter:
                return ShoppingLists.Where(x => x.PlannedShoppingDate > Convert.ToDateTime(filterValue));
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(ShoppingLists), filterBy, null);
        }
    }
}
