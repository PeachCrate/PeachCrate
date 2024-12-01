using Models.Models;

namespace DataLayer.Repositories.Interfaces;

public interface IShoppingListRepository
{
    Task AddShoppingListAsync(ShoppingList shoppingList);
    Task DeleteShoppingListAsync(int shoppingListId);
    Task<IEnumerable<ShoppingList>> GetAllShoppingListsAsync();
    Task<ShoppingList> GetShoppingListByIdAsync(int shoppingListId);
    Task<IEnumerable<ShoppingList>> GetShoppingListsByGroupIdAsync(int groupId);
    Task UpdateShoppingListAsync(ShoppingList shoppingList);
}