using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class CategoryLinqExtensions
{
    public static IQueryable<CategoryDTO> MapCategoryToDTO(this IQueryable<Category> categories)
    {
        return categories.Select(category => new CategoryDTO
        {
            CategoryId = category.CategoryId,
            Title = category.Title,
            Description = category.Description,
            ProductBases = category.ProductBases
                .Select(pb => new ProductBase
                {
                    ProductBaseId = pb.ProductBaseId,
                    Name = pb.Name,
                    Description = pb.Description,
                    Weight = pb.Weight,
                    RunningOutQuantity = pb.RunningOutQuantity,
                })
                .ToList(),
        });
    }

    public static IQueryable<CategoryDTO> OrderCategoriesBy(
        this IQueryable<CategoryDTO> categories,
        CategoryOrderBy orderByOptions
        )
    {
        switch (orderByOptions)
        {
            case CategoryOrderBy.ByCategoryIdDESC:
                return categories.OrderByDescending(category => category.CategoryId);
            case CategoryOrderBy.ByCategoryIdASC:
                return categories.OrderBy(category => category.CategoryId);
            case CategoryOrderBy.ByTitleDESC:
                return categories.OrderByDescending(category => category.Title);
            case CategoryOrderBy.ByTitleASC:
                return categories.OrderBy(category => category.Title);
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }
    public static IQueryable<CategoryDTO> FilterCategoriesBy(
        this IQueryable<CategoryDTO> categories,
        CategoryFilterBy filterBy,
        string filterValue)
    {
        if (string.IsNullOrEmpty(filterValue))
            return categories;

        switch (filterBy)
        {
            case CategoryFilterBy.NoFilter:
                return categories;
            case CategoryFilterBy.ByTitle:
                return categories.Where(x => x.Title.Contains(filterValue));

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(categories), filterBy, null);
        }
    }
}
