using Models.DTOs;
using Models.Models;
using Models.Options;
using Models.Props;

namespace DataLayer.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task AddCategoryAsync(Category category);
    Task AddProductBaseToCategory(ProductBaseCategoryProp prop);
    Task DeleteCategoryAsync(int categoryId);
    Task DeleteCategoryAsync(int categoryId, int changerGroupId);
    Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(CategorySortFilterPageOptions options);
    Task<int> GetCategoriesCountAsync(CategorySortFilterPageOptions options);
    Task<Category> GetCategoryByIdAsync(int categoryId);
    Task RemoveProductBaseFromCategory(ProductBaseCategoryProp prop);
    Task UpdateCategoryAsync(Category category);
    Task UpdateCategoryAsync(Category category, int requesterGroupId);
}