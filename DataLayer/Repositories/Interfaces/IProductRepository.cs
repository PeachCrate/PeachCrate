using Models.DTOs;
using Models.Models;
using Models.Options;

namespace DataLayer.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product> AddProductAsync(ProductDTO product);
    Task DeleteProductAsync(int productId);
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(ProductSortFilterPageOptions options);
    Task<IEnumerable<ProductDTO>> GetAllProductsAsync(ProductSortFilterPageOptions options, int groupId);
    Task<Product> GetProductByIdAsync(int productId);
    Task<int> GetProductCountAsync(ProductSortFilterPageOptions options);
    Task<IEnumerable<Product>> GetProductsByPlaceIdAsync(int placeId);
    Task<List<Product>> GetProductsByValidToDate(int groupId, DateTime validToDate);
    Task UpdateProductAsync(Product product);
}