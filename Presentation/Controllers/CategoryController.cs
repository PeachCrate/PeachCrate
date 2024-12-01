using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Enums;
using Models.Models;
using Models.Options;
using Models.Props;

namespace Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController : ControllerBase
{
    private readonly ICategoryRepository _categoryRepository;
    public CategoryController(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0
        )
    {
        if (!Enum.TryParse(orderBy, out CategoryOrderBy orderByOption))
        {
            orderByOption = CategoryOrderBy.ByCategoryIdDESC;
        }

        if (!Enum.TryParse(filterBy, out CategoryFilterBy filterByOption))
        {
            filterByOption = CategoryFilterBy.NoFilter;
        }

        var options = new CategorySortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        var categories = await _categoryRepository.GetAllCategoriesAsync(options);

        if (categories == null)
        {
            return NotFound();
        }

        return Ok(categories);
    }

    [HttpGet("getCountByOptions")]
    public async Task<ActionResult<int>> GetCategoriesCount(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0
        )
    {
        if (!Enum.TryParse(orderBy, out CategoryOrderBy orderByOption))
            orderByOption = CategoryOrderBy.ByCategoryIdDESC;

        if (!Enum.TryParse(filterBy, out CategoryFilterBy filterByOption))
            filterByOption = CategoryFilterBy.NoFilter;

        var options = new CategorySortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        var count = await _categoryRepository.GetCategoriesCountAsync(options);
        return Ok(count);
    }

    // GET: api/categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(id);
        if (category == null)
            return NotFound();

        return Ok(category);
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        await _categoryRepository.AddCategoryAsync(category);

        return CreatedAtAction("GetCategory", new { id = category.CategoryId }, category);
    }

    // PUT: api/categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category, [FromQuery] int changerGroupId)
    {
        if (id != category.CategoryId)
            return BadRequest();

        await _categoryRepository.UpdateCategoryAsync(category, changerGroupId);

        return NoContent();
    }

    // DELETE: api/categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id, [FromQuery] int changerGroupId)
    {
        await _categoryRepository.DeleteCategoryAsync(id, changerGroupId);
        return NoContent();
    }

    [HttpPut("addProductBaseToCategory")]
    public async Task<IActionResult> AddProductBaseToCategory(ProductBaseCategoryProp prop)
    {
        await _categoryRepository.AddProductBaseToCategory(prop);
        var category = await _categoryRepository.GetCategoryByIdAsync(prop.CategoryId);
        return Ok(category);
    }

    [HttpPut("removeProductBaseFromCategory")]
    public async Task<IActionResult> RemoveProductBaseFromCategory(ProductBaseCategoryProp prop)
    {
        await _categoryRepository.RemoveProductBaseFromCategory(prop);
        var category = await _categoryRepository.GetCategoryByIdAsync(prop.CategoryId);
        return Ok(category);
    }
}