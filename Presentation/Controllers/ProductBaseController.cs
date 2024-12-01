using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models;
using Models.Options;
using Models.Props;
using ServiceLayer.LinqExtensions;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductBaseController : ControllerBase
{
    private readonly IProductBaseRepository _productBaseRepository;

    public ProductBaseController(IProductBaseRepository productBaseRepository)
    {
        _productBaseRepository = productBaseRepository;
    }

    // GET: api/productbases
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductBase>> GetProductBase(int id)
    {
        var productBases = await _productBaseRepository.GetProductBaseByIdAsync(id);
        return Ok(productBases);
    }

    // GET: api/productbases/5
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductBase>>> GetProductBases(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0
        )
    {
        if (!Enum.TryParse(orderBy, out ProductBaseOrderBy orderByOption))
            orderByOption = ProductBaseOrderBy.ByProductBaseIdASC;

        if (!Enum.TryParse(filterBy, out ProductBaseFilterBy filterByOption))
            filterByOption = ProductBaseFilterBy.NoFilter;

        var options = new ProductBaseSortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        var productBasesDTO = await _productBaseRepository.GetAllProductBasesAsync(options);

        if (productBasesDTO == null)
            return NotFound();

        var productBases = productBasesDTO.AsQueryable().MapDTOToProductBase().ToList();
        return Ok(productBases.RemoveRecursion());
    }

    [HttpGet("getCountByOptions")]
    public async Task<ActionResult<int>> GetProductBasesCount(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0
        )
    {
        if (!Enum.TryParse(orderBy, out ProductBaseOrderBy orderByOption))
            orderByOption = ProductBaseOrderBy.ByProductBaseIdASC;

        if (!Enum.TryParse(filterBy, out ProductBaseFilterBy filterByOption))
            filterByOption = ProductBaseFilterBy.NoFilter;

        var options = new ProductBaseSortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        var count = await _productBaseRepository.GetProductBasesCountAsync(options);
        return Ok(count);
    }

    // POST: api/productbases
    [HttpPost]
    public async Task<ActionResult<ProductBase>> PostProductBase(ProductBase productBase)
    {
        await _productBaseRepository.AddProductBaseAsync(productBase);
        return CreatedAtAction("GetProductBase", new { id = productBase.ProductBaseId }, productBase.RemoveRecursion());
    }

    // PUT: api/productbases/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProductBase(int id, ProductBase productBase, [FromQuery] int changerGroupId)
    {
        if (id != productBase.ProductBaseId)
        {
            return BadRequest();
        }
        try
        {
            await _productBaseRepository.UpdateProductBaseAsync(productBase, changerGroupId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

        return NoContent();
    }

    // DELETE: api/productbases/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProductBase(int id, [FromQuery] int changerGroupId)
    {
        await _productBaseRepository.DeleteProductBaseAsync(id, changerGroupId);
        return NoContent();
    }

    [HttpPut("addCategoryToProductBase")]
    public async Task<IActionResult> AddCategoryToProductBase(ProductBaseCategoryProp prop)
    {
        await _productBaseRepository.AddCategoryToProductBase(prop);
        var productBase = await _productBaseRepository.GetProductBaseByIdAsync(prop.ProductBaseId);
        return Ok(productBase);
    }
    [HttpPut("removeCategoryFromProductBase")]
    public async Task<IActionResult> RemoveCategoryFromProductBase(ProductBaseCategoryProp prop)
    {
        await _productBaseRepository.RemoveCategoryFromProductBase(prop);
        var productBase = await _productBaseRepository.GetProductBaseByIdAsync(prop.ProductBaseId);
        return Ok(productBase);
    }

}