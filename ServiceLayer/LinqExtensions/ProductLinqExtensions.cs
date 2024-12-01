using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class ProductLinqExtension
{
    public static IQueryable<ProductDTO> MapProductToDTO(this IQueryable<Product> products)
    {

        return products.Select(product => new ProductDTO
        {
            ProductId = product.ProductId,
            ProductBaseId = product.ProductBaseId,
            ProductBase = product.ProductBase,
            Quantity = product.Quantity,
            Price = product.Price,
            PurchaseDate = product.PurchaseDate,
            ValidUntil = product.ValidUntil,
            PlaceId = product.PlaceId,
            Place = product.Place,
        });
    }
    public static IQueryable<Product> MapDTOToProduct(this IQueryable<ProductDTO> productDTOsDTO)
    {

        return productDTOsDTO.Select(productDTO => new Product
        {
            ProductId = productDTO.ProductId,
            ProductBaseId = productDTO.ProductBaseId,
            ProductBase = productDTO.ProductBase,
            Quantity = productDTO.Quantity,
            Price = productDTO.Price,
            PurchaseDate = productDTO.PurchaseDate,
            ValidUntil = productDTO.ValidUntil,
            PlaceId = productDTO.PlaceId,
            Place = productDTO.Place,
        });
    }

    public static Product RemoveRecursion(this Product product)
    {
        if (product.ProductBase != null)
        {
            product.ProductBase.Products = null;
            product.ProductBase.ShoppingProducts = null;
            if (product.ProductBase.Categories != null)
            {
                product.ProductBase.Categories.ForEach(c => c.ProductBases = null);
            }
        }
        if (product.Place != null)
        {
            var place = product.Place;
            product.Place = new Place
            {
                PlaceId = place.PlaceId,
                Name = place.Name,
                Description = place.Description,
                Location = null,
                Products = null,
            };
        }
        return product;
    }
    public static List<Product> RemoveRecursion(this List<Product> products)
    {
        products.ForEach(p => p.RemoveRecursion());
        return products;
    }

    public static IQueryable<ProductDTO> OrderProductsBy(
        this IQueryable<ProductDTO> products,
        ProductOrderBy orderByOptions
        )
    {
        switch (orderByOptions)
        {
            case ProductOrderBy.ByProductIdDESC:
                return products.OrderByDescending(x => x.ProductId);
            case ProductOrderBy.ByQuantityDESC:
                return products.OrderByDescending(x => x.Quantity);
            case ProductOrderBy.ByPriceDESC:
                return products.OrderByDescending(x => x.Price);

            case ProductOrderBy.ByProductBaseIdDESC:
                return products.OrderByDescending(x => x.ProductBaseId);
            case ProductOrderBy.ByNameDESC:
                return products.OrderByDescending(x => x.ProductBase.Name);
            case ProductOrderBy.ByWeightDESC:
                return products.OrderByDescending(x => x.ProductBase.Weight);


            case ProductOrderBy.ByPlaceIdDESC:
                return products.OrderByDescending(place => place.PlaceId);
            case ProductOrderBy.ByPlaceNameDESC:
                return products.OrderByDescending(place => place.Place.Name);
            case ProductOrderBy.ByPlaceIdASC:
                return products.OrderBy(place => place.PlaceId);
            case ProductOrderBy.ByPlaceNameASC:
                return products.OrderBy(place => place.Place.Name);



            case ProductOrderBy.ByProductBaseIdASC:
                return products.OrderBy(x => x.ProductBaseId);
            case ProductOrderBy.ByNameASC:
                return products.OrderBy(x => x.ProductBase.Name);
            case ProductOrderBy.ByWeightASC:
                return products.OrderBy(x => x.ProductBase.Weight);


            case ProductOrderBy.ByProductIdASC:
                return products.OrderBy(x => x.ProductId);
            case ProductOrderBy.ByQuantityASC:
                return products.OrderBy(x => x.Quantity);
            case ProductOrderBy.ByPriceASC:
                return products.OrderBy(x => x.Price);
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }
    public static IQueryable<ProductDTO> FilterProductsBy(
        this IQueryable<ProductDTO> products,
        ProductFilterBy filterBy,
        string filterValue
        )
    {
        if (string.IsNullOrEmpty(filterValue))
            return products;

        switch (filterBy)
        {
            case ProductFilterBy.NoFilter:
                return products;
            case ProductFilterBy.ByQuantityBiggerThan:
                return products.Where(x => x.Quantity > Convert.ToInt32(filterValue));
            case ProductFilterBy.ByQuantitySmallerThan:
                return products.Where(x => x.Quantity < Convert.ToInt32(filterValue));

            case ProductFilterBy.ByPriceBiggerThan:
                return products.Where(x => x.Price > Convert.ToDouble(filterValue));
            case ProductFilterBy.ByPriceSmallerThan:
                return products.Where(x => x.Price < Convert.ToDouble(filterValue));


            case ProductFilterBy.ByName:
                return products.Where(x => x.ProductBase.Name.Contains(filterValue));
            case ProductFilterBy.ByWeightBiggerThan:
                return products.Where(x => x.ProductBase.Weight > Convert.ToDouble(filterValue));
            case ProductFilterBy.ByWeightSmallerThan:
                return products.Where(x => x.ProductBase.Weight < Convert.ToDouble(filterValue));

            case ProductFilterBy.ByPlaceName:
                return products.Where(x => x.Place.Name.Contains(filterValue));

            case ProductFilterBy.ByValidUntilBefore:
                var dateBefore = Convert.ToDateTime(filterValue);
                return products.Where(x => x.ValidUntil < dateBefore);
            case ProductFilterBy.ByValidUntilAfter:
                var dateAfter = Convert.ToDateTime(filterValue);
                return products.Where(x => x.ValidUntil > dateAfter);


            case ProductFilterBy.ByPurchaseDateBefore:
                var date = Convert.ToDateTime(filterValue);
                return products.Where(x => x.PurchaseDate < date);
            case ProductFilterBy.ByPurchaseDateAfter:
                var date1 = Convert.ToDateTime(filterValue);
                return products.Where(x => x.PurchaseDate > date1);

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(products), filterBy, null);
        }
    }
}
