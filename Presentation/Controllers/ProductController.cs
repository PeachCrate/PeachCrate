using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Enums;
using Models.Models;
using Models.Options;
using ServiceLayer.LinqExtensions;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0,
        [FromQuery] int? groupId = null
        )
    {
        if (!Enum.TryParse(orderBy, out ProductOrderBy orderByOption))
            orderByOption = ProductOrderBy.ByProductBaseIdASC;

        if (!Enum.TryParse(filterBy, out ProductFilterBy filterByOption))
            filterByOption = ProductFilterBy.NoFilter;

        var options = new ProductSortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        
        if (groupId != null)
        {
            var productsByGroupId = await _productRepository.GetAllProductsAsync(options, groupId.Value);
            var productsList = productsByGroupId.AsQueryable().MapDTOToProduct().ToList();
            productsList.ForEach(p =>
            {
                p.ProductBase?.Categories?.ForEach(c => c.ProductBases = null);
                p.Place.Products = null;
                p.Place.Location.Places = null;
                p.Place.Location.Group.Locations = null;
                p.Place.Location.Group.ShoppingLists = null;
            });
            return Ok(productsList);
        }
        
        var products = await _productRepository.GetAllProductsAsync(options);
        var removedRecursion1 = products.AsQueryable().MapDTOToProduct().ToList().RemoveRecursion();
        return Ok(removedRecursion1);
    }

    [HttpGet("getCountByOptions")]
    public async Task<ActionResult<int>> GetProductCount(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0
        )
    {
        if (!Enum.TryParse(orderBy, out ProductOrderBy orderByOption))
            orderByOption = ProductOrderBy.ByProductBaseIdASC;

        if (!Enum.TryParse(filterBy, out ProductFilterBy filterByOption))
            filterByOption = ProductFilterBy.NoFilter;

        var options = new ProductSortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        var count = await _productRepository.GetProductCountAsync(options);
        return Ok(count);
    }

    // GET: api/products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpGet("getProductsByPlaceId/{placeId}")]
    public async Task<ActionResult<List<Product>>> GetProductsByPlaceId(int placeId)
    {
        var products = await _productRepository.GetProductsByPlaceIdAsync(placeId);

        if (products == null)
            return NotFound();
        var productList = products.ToList();
        var noRecursionProducts = productList.RemoveRecursion();
        return Ok(noRecursionProducts);
    }
    
    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(ProductDTO productDTO)
    {
        var product = await _productRepository.AddProductAsync(productDTO);
        var productNoRecursion = product.RemoveRecursion();
        return CreatedAtAction("GetProduct", new { id = product.ProductId }, productNoRecursion);
    }

    // PUT: api/products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(int id, Product product)
    {
        if (id != product.ProductId)
        {
            return BadRequest();
        }
        await _productRepository.UpdateProductAsync(product);
        return NoContent();
    }

    // DELETE: api/products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        await _productRepository.DeleteProductAsync(id);
        return NoContent();
    }
}