using Models.Models;

namespace DataLayer.Repositories.Interfaces;

public interface IShoppingProductRepository
{
    Task AddShoppingProductAsync(ShoppingProduct shoppingProduct);
    Task DeleteShoppingProductAsync(int shoppingProductId);
    Task<IEnumerable<ShoppingProduct>> GetAllShoppingProductsAsync();
    Task<ShoppingProduct> GetShoppingProductByIdAsync(int shoppingProductId);
    Task<IEnumerable<ShoppingProduct>> GetShoppingProductsByShoppingListId(int shoppingProductId);
    Task UpdateShoppingProductAsync(ShoppingProduct shoppingProduct);
}