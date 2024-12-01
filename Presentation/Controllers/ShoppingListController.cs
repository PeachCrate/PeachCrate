using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using ServiceLayer.LinqExtensions;

namespace Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ShoppingListController : ControllerBase
{
    private readonly IShoppingListRepository _shoppingListRepository;

    public ShoppingListController(IShoppingListRepository shoppingListRepository)
    {
        _shoppingListRepository = shoppingListRepository;
    }

    // GET: api/shoppinglists
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShoppingList>>> GetShoppingLists()
    {
        var shoppingLists = await _shoppingListRepository.GetAllShoppingListsAsync();
        return Ok(shoppingLists);
    }

    // GET: api/shoppinglists/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ShoppingList>> GetShoppingList(int id)
    {
        var shoppingList = await _shoppingListRepository.GetShoppingListByIdAsync(id);

        if (shoppingList == null)
            return NotFound();

        return Ok(shoppingList.RemoveRecursion());
    }
    
    [HttpGet("getShoppingListsByGroupId/{groupId}")]
    public async Task<ActionResult<List<ShoppingList>>> GetShoppingListsByGroupIdAsync(int groupId)
    {
        var shoppingLists = await _shoppingListRepository.GetShoppingListsByGroupIdAsync(groupId);
        var shoppingListsNoRecursion = shoppingLists.ToList().RemoveRecursion();
        return Ok(shoppingListsNoRecursion);
    }

    // POST: api/shoppinglists
    [HttpPost]
    public async Task<ActionResult<ShoppingList>> PostShoppingList(ShoppingList shoppingList)
    {
        await _shoppingListRepository.AddShoppingListAsync(shoppingList);
        return CreatedAtAction("GetShoppingList", new { id = shoppingList.ShoppingListId }, shoppingList.RemoveRecursion());
    }

    // PUT: api/shoppinglists/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutShoppingList(int id, ShoppingList shoppingList)
    {
        if (id != shoppingList.ShoppingListId)
            return BadRequest();

        await _shoppingListRepository.UpdateShoppingListAsync(shoppingList);
        return NoContent();
    }

    // DELETE: api/shoppinglists/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShoppingList(int id)
    {
        await _shoppingListRepository.DeleteShoppingListAsync(id);
        return NoContent();
    }
}
