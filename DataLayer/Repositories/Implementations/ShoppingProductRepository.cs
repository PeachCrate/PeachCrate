using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DataLayer.Repositories.Implementations;
public class ShoppingProductRepository : IShoppingProductRepository
{
    private readonly DataContext _dataContext;

    public ShoppingProductRepository(DataContext context)
    {
        _dataContext = context;
    }

    // Create
    public async Task AddShoppingProductAsync(ShoppingProduct shoppingProduct)
    {
        if (shoppingProduct == null)
        {
            throw new ArgumentNullException(nameof(shoppingProduct));
        }
        var shoppingList = await _dataContext.ShoppingLists.FindAsync(shoppingProduct.ShoppingList.ShoppingListId);
        var productBase = await _dataContext.ProductBases.FindAsync(shoppingProduct.ProductBase.ProductBaseId);

        shoppingProduct.ShoppingList = shoppingList;
        shoppingProduct.ProductBase = productBase;

        _dataContext.ShoppingProducts.Add(shoppingProduct);
        await _dataContext.SaveChangesAsync();
    }

    // Read
    public async Task<ShoppingProduct> GetShoppingProductByIdAsync(int shoppingProductId)
    {
        return await _dataContext.ShoppingProducts.FindAsync(shoppingProductId);
    }

    public async Task<IEnumerable<ShoppingProduct>> GetAllShoppingProductsAsync()
    {
        return await _dataContext.ShoppingProducts.ToListAsync();
    }
    public async Task<IEnumerable<ShoppingProduct>> GetShoppingProductsByShoppingListId(int shoppingProductId)
    {
        var shoppingProducts = await _dataContext.ShoppingProducts
            .Include(sp => sp.ShoppingList)
            .Include(sp => sp.ProductBase)
            .Where(sp => sp.ShoppingListId == shoppingProductId)
            .ToListAsync();
        return shoppingProducts;

    }

    // Update
    public async Task UpdateShoppingProductAsync(ShoppingProduct shoppingProduct)
    {
        if (shoppingProduct == null)
            throw new ArgumentNullException(nameof(shoppingProduct));

        _dataContext.Entry(shoppingProduct).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    // Delete
    public async Task DeleteShoppingProductAsync(int shoppingProductId)
    {
        var shoppingProduct = await _dataContext.ShoppingProducts.FindAsync(shoppingProductId);
        if (shoppingProduct != null)
        {
            _dataContext.ShoppingProducts.Remove(shoppingProduct);
            await _dataContext.SaveChangesAsync();
        }
    }
}