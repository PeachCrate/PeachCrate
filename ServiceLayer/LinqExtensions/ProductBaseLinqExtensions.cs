using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class ProductBaseLinqExtension
{
    public static IQueryable<ProductBaseDTO> MapProductBaseToDTO(this IQueryable<ProductBase> productBases)
    {

        return productBases.Select(pb => new ProductBaseDTO
        {
            ProductBaseId = pb.ProductBaseId,
            Name = pb.Name,
            Description = pb.Description,
            Weight = pb.Weight,
            RunningOutQuantity = pb.RunningOutQuantity,
            Categories = pb.Categories
                .Select(category => new Category
                {
                    CategoryId = category.CategoryId,
                    Title = category.Title,
                    Description = category.Description
                })
                .ToList(),
        });
    }
    public static IQueryable<ProductBase> MapDTOToProductBase(this IQueryable<ProductBaseDTO> productBasesDTO)
    {

        return productBasesDTO.Select(pb => new ProductBase
        {
            ProductBaseId = pb.ProductBaseId,
            Name = pb.Name,
            Description = pb.Description,
            Weight = pb.Weight,
            RunningOutQuantity = pb.RunningOutQuantity,
            Categories = pb.Categories
                .Select(category => new Category
                {
                    CategoryId = category.CategoryId,
                    Title = category.Title,
                    Description = category.Description
                })
                .ToList(),
        });
    }
    public static ProductBase RemoveRecursion(this ProductBase productBase)
    {
        if (productBase.Categories != null)
        {
            productBase.Categories = productBase.Categories
                .Select(c => new Category
                {
                    CategoryId = c.CategoryId,
                    Title = c.Title,
                    Description = c.Description,
                }).ToList();
        }
        return productBase;
    }
    public static List<ProductBase> RemoveRecursion(this List<ProductBase> productBases)
    {
        productBases.ForEach(pb => pb.RemoveRecursion());
        return productBases;
    }


    public static IQueryable<ProductBaseDTO> OrderProductBasesBy(this IQueryable<ProductBaseDTO> productBases,
        ProductBaseOrderBy orderByOptions)
    {
        switch (orderByOptions)
        {
            case ProductBaseOrderBy.ByProductBaseIdDESC:
                return productBases.OrderByDescending(x => x.ProductBaseId);
            case ProductBaseOrderBy.ByNameDESC:
                return productBases.OrderByDescending(x => x.Name);
            case ProductBaseOrderBy.ByWeightDESC:
                return productBases.OrderByDescending(x => x.Weight);
            case ProductBaseOrderBy.ByProductBaseIdASC:
                return productBases.OrderBy(x => x.ProductBaseId);
            case ProductBaseOrderBy.ByNameASC:
                return productBases.OrderBy(x => x.Name);
            case ProductBaseOrderBy.ByWeightASC:
                return productBases.OrderBy(x => x.Weight);
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }



    public static IQueryable<ProductBaseDTO> FilterProductBasesBy(this IQueryable<ProductBaseDTO> productBases,
        ProductBaseFilterBy filterBy,
        string filterValue)
    {
        if (string.IsNullOrEmpty(filterValue))
            return productBases;

        switch (filterBy)
        {
            case ProductBaseFilterBy.NoFilter:
                return productBases;
            case ProductBaseFilterBy.ByName:
                return productBases.Where(x => x.Name.Contains(filterValue));
            case ProductBaseFilterBy.ByWeightSmallerThan:
                return productBases.Where(x => x.Weight < Convert.ToDouble(filterValue));
            case ProductBaseFilterBy.ByWeightBiggerThan:
                return productBases.Where(x => x.Weight > Convert.ToDouble(filterValue));



            default:
                throw new ArgumentOutOfRangeException(
                    nameof(productBases), filterBy, null);
        }
    }



}