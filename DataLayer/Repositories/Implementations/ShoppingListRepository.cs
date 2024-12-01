using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;

namespace DataLayer.Repositories.Implementations;

public class ShoppingListRepository : IShoppingListRepository
{
    private readonly DataContext _dataContext;

    public ShoppingListRepository(DataContext context)
    {
        _dataContext = context;
    }

    // Read
    public async Task<ShoppingList> GetShoppingListByIdAsync(int shoppingListId)
    {
        var shoppingList = await _dataContext.ShoppingLists.FindAsync(shoppingListId);
        if (shoppingList == null)
            return null;
        await _dataContext.Entry(shoppingList).Reference(sl => sl.Group).LoadAsync();

        await _dataContext.Entry(shoppingList).Collection(sl => sl.ShoppingProducts).LoadAsync();
        if (shoppingList.ShoppingProducts != null)
            shoppingList.ShoppingProducts.ForEach(sp => _dataContext.Entry(sp).Reference(sp => sp.ProductBase).Load());
        return shoppingList;
    }

    public async Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync()
    {
        return await _dataContext.ShoppingLists.ToListAsync();
    }

    public async Task<IEnumerable<ShoppingList>> GetShoppingListsByGroupIdAsync(int groupId)
    {
        var group = await _dataContext.Groups.FindAsync(groupId);
        if (group == null)
            return null;

        await _dataContext.Entry(group).Collection(g => g.ShoppingLists).LoadAsync();
        if (group.ShoppingLists == null)
            return null;

        group.ShoppingLists.ForEach(sl => _dataContext.Entry(sl).Collection(sl => sl.ShoppingProducts).LoadAsync());
        return group.ShoppingLists;
    }

    // Create
    public async Task AddShoppingListAsync(ShoppingList shoppingList)
    {
        if (shoppingList == null)
            throw new ArgumentNullException(nameof(shoppingList));

        if (shoppingList.Group != null)
            shoppingList.Group = await _dataContext.Groups.FindAsync(shoppingList.Group.GroupId.Value);

        _dataContext.ShoppingLists.Add(shoppingList);
        await _dataContext.SaveChangesAsync();
    }


    // Update
    public async Task UpdateShoppingListAsync(ShoppingList shoppingList)
    {
        if (shoppingList == null)
            throw new ArgumentNullException(nameof(shoppingList));

        if (shoppingList.Group != null)
            shoppingList.Group = await _dataContext.Groups.FindAsync(shoppingList.Group.GroupId.Value);

        _dataContext.Entry(shoppingList).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    // Delete
    public async Task DeleteShoppingListAsync(int shoppingListId)
    {
        var shoppingList = await _dataContext.ShoppingLists.FindAsync(shoppingListId);
        if (shoppingList == null)
            return;
        
        _dataContext.ShoppingLists.Remove(shoppingList);
        await _dataContext.SaveChangesAsync();
    }
}