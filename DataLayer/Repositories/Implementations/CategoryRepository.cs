using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Models;
using Models.Options;
using Models.Props;
using ServiceLayer.LinqExtensions;

namespace DataLayer.Repositories.Implementations;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _dataContext;

    public CategoryRepository(DataContext context)
    {
        _dataContext = context;
    }

    // Create
    public async Task AddCategoryAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        category.CategoryId = 0;
        _dataContext.Categories.Add(category);
        await _dataContext.SaveChangesAsync();
    }

    // Read
    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        var category = await _dataContext.Categories
            .Include(c => c.ProductBases)
            .Where(c => c.CategoryId == categoryId)
            .SingleOrDefaultAsync();
        if (category != null && category.ProductBases != null)
        {
            foreach (var productBase in category.ProductBases)
            {
                productBase.Categories = null;
            }
        }

        return category;
    }

    public async Task<IEnumerable<CategoryDTO>> GetAllCategoriesAsync(CategorySortFilterPageOptions options)
    {
        var categories = await _dataContext.Categories
            .AsNoTracking()
            .Include(category => category.ProductBases)
            .MapCategoryToDTO()
            .OrderCategoriesBy(options.OrderBy)
            .FilterCategoriesBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToListAsync();
        return categories;
    }

    public async Task<int> GetCategoriesCountAsync(CategorySortFilterPageOptions options)
    {
        var count = _dataContext.Categories
            .AsNoTracking()
            .MapCategoryToDTO()
            .OrderCategoriesBy(options.OrderBy)
            .FilterCategoriesBy(options.FilterBy, options.FilterValue)
            .Count();
        int countRes = Convert.ToInt32(count);
        return countRes;
    }

    // Update
    public async Task UpdateCategoryAsync(Category category)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        _dataContext.Entry(category).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateCategoryAsync(Category category, int requesterGroupId)
    {
        if (category == null)
        {
            throw new ArgumentNullException(nameof(category));
        }

        int categoryId = category.CategoryId;

        var groupsByCategoryId = await _dataContext.Groups
            .Include(group => group.Locations)!
            .ThenInclude(location => location.Places)!
            .ThenInclude(place => place.Products)!
            .ThenInclude(product => product.ProductBase)
            .ThenInclude(productBase => productBase.Categories)
            .Where(g => g.Locations.Any(l =>
                l.Places.Any(p => p.Products.Any(p => p.ProductBase.Categories.Any(c => c.CategoryId == categoryId)))))
            .ToListAsync();

        bool isOtherGroupContainsCategory = false;

        foreach (var group in groupsByCategoryId)
            if (group.GroupId != requesterGroupId)
            {
                isOtherGroupContainsCategory = true;
                break;
            }

        if (!isOtherGroupContainsCategory)
        {
            // Якщо не має залежних груп за записів, то просто змінюємо
            _dataContext.Entry(category).State = EntityState.Modified;
            await _dataContext.SaveChangesAsync();
            return;
        }

        // Продукт бази які прив'язані до змінюваної категорії і до запитуваної групи
        var productBases = await _dataContext.ProductBases
            .Include(productBase => productBase.Categories)
            .Include(productBase => productBase.Products)
            .ThenInclude(product => product.Place)
            .ThenInclude(place => place.Location)
            .ThenInclude(location => location.Group)
            .Where(pb =>
                pb.Categories.Any(c => c.CategoryId == categoryId) && pb.Products.Any(product =>
                    product.Place.Location.Group.GroupId == requesterGroupId))
            .ToListAsync();

        var loadedCategory = await _dataContext.Categories.FindAsync(category.CategoryId);
        if (loadedCategory == null)
            throw new ArgumentException(nameof(category));
        await _dataContext.Entry(category).Collection(c => c.ProductBases).LoadAsync();

        var loadedProductBases = new List<ProductBase>();
        foreach (var productBase in productBases)
        {
            var loadedProductBase = await _dataContext.ProductBases.FindAsync(productBase.ProductBaseId);
            if (loadedProductBase != null)
            {
                await _dataContext.Entry(loadedProductBase).Collection(pb => pb.Categories).LoadAsync();
                loadedProductBase.Categories.Remove(loadedCategory);
                loadedProductBases.Add(loadedProductBase);
            }
        }

        var newCategory = new Category
        {
            CategoryId = 0,
            Title = category.Title,
            Description = category.Description,
        };
        newCategory.ProductBases = loadedProductBases;

        await _dataContext.AddAsync(newCategory);
        await _dataContext.SaveChangesAsync();
    }

    // Delete
    public async Task DeleteCategoryAsync(int categoryId)
    {
        var category = await _dataContext.Categories.FindAsync(categoryId);
        if (category == null)
            throw new ArgumentException("Category not found");

        _dataContext.Categories.Remove(category);
        await _dataContext.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int categoryId, int changerGroupId)
    {
        var category = await _dataContext.Categories.FindAsync(categoryId);
        if (category == null)
            throw new ArgumentException("Category not found");

        var groupsByCategoryId = await _dataContext.Groups
            .Include(group => group.Locations)!
            .ThenInclude(location => location.Places)!
            .ThenInclude(place => place.Products)!
            .ThenInclude(product => product.ProductBase)
            .ThenInclude(productBase => productBase.Categories)
            .Where(g => g.Locations.Any(l =>
                l.Places.Any(p => p.Products.Any(p => p.ProductBase.Categories.Any(c => c.CategoryId == categoryId)))))
            .ToListAsync();

        bool isOtherGroupContainsCategory = false;
        foreach (var group in groupsByCategoryId)
            if (group.GroupId != changerGroupId)
            {
                isOtherGroupContainsCategory = true;
                break;
            }

        if (!isOtherGroupContainsCategory)
        {
            // Якщо не має залежних груп за записів, то просто змінюємо
            await DeleteCategoryAsync(categoryId);
            return;
        }

        throw new Exception("Other groups depends on this category");
    }

    public async Task AddProductBaseToCategory(ProductBaseCategoryProp prop)
    {
        var category = await _dataContext.Categories
            .Where(category => category.CategoryId == prop.CategoryId)
            .SingleOrDefaultAsync();
        var productBase = await _dataContext.ProductBases
            .Include(pb => pb.Categories)
            .Where(pb => pb.ProductBaseId == prop.ProductBaseId)
            .SingleOrDefaultAsync();

        if (productBase == null || category == null)
            return;

        category.ProductBases.Add(productBase);
        await _dataContext.SaveChangesAsync();
    }

    public async Task RemoveProductBaseFromCategory(ProductBaseCategoryProp prop)
    {
        var category = await _dataContext.Categories
            .Where(category => category.CategoryId == prop.CategoryId)
            .SingleOrDefaultAsync();
        var productBase = await _dataContext.ProductBases
            .Include(pb => pb.Categories)
            .Where(pb => pb.ProductBaseId == prop.ProductBaseId)
            .SingleOrDefaultAsync();

        if (productBase == null || category == null)
            return;

        category.ProductBases.Remove(productBase);
        await _dataContext.SaveChangesAsync();
    }
}