using Models.DTOs;
using Models.Models;
using Models.Options;
using Models.Props;

namespace DataLayer.Repositories.Interfaces;

public interface IProductBaseRepository
{
    Task AddCategoryToProductBase(ProductBaseCategoryProp prop);
    Task AddProductBaseAsync(ProductBase productBase);
    Task DeleteProductBaseAsync(int productBaseId);
    Task DeleteProductBaseAsync(int productBaseId, int groupId);
    Task<IEnumerable<ProductBaseDTO>> GetAllProductBasesAsync(ProductBaseSortFilterPageOptions options);
    Task<ProductBase> GetProductBaseByIdAsync(int productBaseId);
    Task<int> GetProductBasesCountAsync(ProductBaseSortFilterPageOptions options);
    Task RemoveCategoryFromProductBase(ProductBaseCategoryProp prop);
    Task UpdateProductBaseAsync(ProductBase productBase);
    Task UpdateProductBaseAsync(ProductBase productBase, int groupId);
}