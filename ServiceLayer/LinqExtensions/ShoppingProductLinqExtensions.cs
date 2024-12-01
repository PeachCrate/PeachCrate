using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class ShoppingProductLinqExtensions
{
    public static IQueryable<ShoppingProductDTO> MapShoppingProductsToDTO(this IQueryable<ShoppingProduct> shoppingProducts)
    {
        return shoppingProducts.Select(sp => new ShoppingProductDTO
        {
            ShoppingProductId = sp.ShoppingProductId,
            ProductBaseId = sp.ProductBaseId,
            ProductBase = sp.ProductBase,
            Quantity = sp.Quantity,
            ShoppingListId = sp.ShoppingListId,
            ShoppingList = sp.ShoppingList,
        });
    }
    public static IQueryable<ShoppingProduct> MapDTOToShoppingProducts(this IQueryable<ShoppingProductDTO> shoppingProducts)
    {
        return shoppingProducts.Select(sp => new ShoppingProduct
        {
            ShoppingProductId = sp.ShoppingProductId,
            ProductBaseId = sp.ProductBaseId,
            ProductBase = sp.ProductBase,
            Quantity = sp.Quantity,
            ShoppingListId = sp.ShoppingListId.Value,
            ShoppingList = sp.ShoppingList,
        });
    }

    public static ShoppingProduct RemoveRecursion(this ShoppingProduct shoppingProduct)
    {
        if (shoppingProduct.ProductBase != null)
        {
            shoppingProduct.ProductBase.Products = null;
            shoppingProduct.ProductBase.ShoppingProducts = null;
            shoppingProduct.ProductBase.Categories?.ForEach(c => c.ProductBases = null);

        }
        if (shoppingProduct.ShoppingList != null)
        {
            shoppingProduct.ShoppingList.ShoppingProducts = null;
            shoppingProduct.ShoppingList.Group = null;
        }
        return shoppingProduct;
    }
    public static List<ShoppingProduct> RemoveRecursion(this List<ShoppingProduct> shoppingProduct)
    {
        shoppingProduct.ForEach(shoppingList => shoppingList.RemoveRecursion());
        return shoppingProduct;
    }
    public static IQueryable<ShoppingProductDTO> OrderShoppingProductsBy(
        this IQueryable<ShoppingProductDTO> shoppingLists,
        ShoppingProductOrderBy orderByOptions
        )
    {
        switch (orderByOptions)
        {
            case ShoppingProductOrderBy.ByProductIdDESC:
                return shoppingLists.OrderByDescending(sl => sl.ShoppingProductId);
            case ShoppingProductOrderBy.ByProductIdASC:
                return shoppingLists.OrderBy(sl => sl.ShoppingProductId);

            case ShoppingProductOrderBy.ByQuantityDESC:
                return shoppingLists.OrderByDescending(sl => sl.Quantity);
            case ShoppingProductOrderBy.ByQuantityASC:
                return shoppingLists.OrderBy(sl => sl.Quantity);

            case ShoppingProductOrderBy.ByNameDESC:
                return shoppingLists.OrderByDescending(sl => sl.ProductBase.Name);
            case ShoppingProductOrderBy.ByNameASC:
                return shoppingLists.OrderBy(sl => sl.ProductBase.Name);

            case ShoppingProductOrderBy.ByWeightDESC:
                return shoppingLists.OrderByDescending(sl => sl.ProductBase.Weight);
            case ShoppingProductOrderBy.ByWeightASC:
                return shoppingLists.OrderBy(sl => sl.ProductBase.Weight);

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }
    public static IQueryable<ShoppingProductDTO> FilterShoppingProductsBy(
        this IQueryable<ShoppingProductDTO> ShoppingProducts,
        ShoppingProductFilterBy filterBy,
        string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
            return ShoppingProducts;

        switch (filterBy)
        {
            case ShoppingProductFilterBy.NoFilter:
                return ShoppingProducts;
            case ShoppingProductFilterBy.ByName:
                return ShoppingProducts.Where(x => x.ProductBase.Name.Contains(filterValue));
            case ShoppingProductFilterBy.ByQuantitySmallerThan:
                return ShoppingProducts.Where(x => x.Quantity < Convert.ToInt32(filterValue));
            case ShoppingProductFilterBy.ByQuantityBiggerThan:
                return ShoppingProducts.Where(x => x.Quantity > Convert.ToInt32(filterValue));

            case ShoppingProductFilterBy.ByWeightSmallerThan:
                return ShoppingProducts.Where(x => x.ProductBase.Weight < Convert.ToInt32(filterValue));

            case ShoppingProductFilterBy.ByWeightBiggerThan:
                return ShoppingProducts.Where(x => x.ProductBase.Weight > Convert.ToInt32(filterValue));

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(ShoppingProducts), filterBy, null);
        }
    }
}
