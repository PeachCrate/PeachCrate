using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using ServiceLayer.LinqExtensions;


namespace Presentation.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ShoppingProductController : ControllerBase
{
    private readonly IShoppingProductRepository _shoppingProductRepository;

    public ShoppingProductController(IShoppingProductRepository shoppingProductRepository)
    {
        _shoppingProductRepository = shoppingProductRepository;
    }

    // GET: api/shoppinglistproducts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShoppingProduct>>> GetShoppingProducts()
    {
        var shoppingProducts = await _shoppingProductRepository.GetAllShoppingProductsAsync();
        return Ok(shoppingProducts);
    }

    // GET: api/shoppinglistproducts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ShoppingProduct>> GetShoppingProduct(int id)
    {
        var shoppingProduct = await _shoppingProductRepository.GetShoppingProductByIdAsync(id);

        if (shoppingProduct == null)
            return NotFound();

        return Ok(shoppingProduct);
    }
    [HttpGet("getShoppingProductsByShoppingListId/{shoppingListId}")]
    public async Task<ActionResult<List<ShoppingProduct>>> GetShoppingProductsByShoppingListId(int shoppingListId)
    {
        var shoppingProducts = await _shoppingProductRepository.GetShoppingProductsByShoppingListId(shoppingListId);
        var shoppingProductsNoRecursion = shoppingProducts.ToList().RemoveRecursion();
        return Ok(shoppingProductsNoRecursion);
    }

    // POST: api/shoppinglistproducts
    [HttpPost]
    public async Task<ActionResult<ShoppingProduct>> PostShoppingProduct(ShoppingProduct shoppingProduct)
    {
        await _shoppingProductRepository.AddShoppingProductAsync(shoppingProduct);
        return CreatedAtAction("GetShoppingProduct", new { id = shoppingProduct.ShoppingProductId }, shoppingProduct.RemoveRecursion());
    }

    // PUT: api/shoppinglistproducts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutShoppingProduct(int id, ShoppingProduct shoppingProduct)
    {
        if (id != shoppingProduct.ShoppingProductId)
            return BadRequest();

        await _shoppingProductRepository.UpdateShoppingProductAsync(shoppingProduct);
        return NoContent();
    }

    // DELETE: api/shoppinglistproducts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShoppingProduct(int id)
    {
        await _shoppingProductRepository.DeleteShoppingProductAsync(id);

        return NoContent();
    }

}