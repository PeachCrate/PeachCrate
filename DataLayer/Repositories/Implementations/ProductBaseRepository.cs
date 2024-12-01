using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Models;
using Models.Options;
using Models.Props;
using ServiceLayer.LinqExtensions;

namespace DataLayer.Repositories.Implementations;

public class ProductBaseRepository : IProductBaseRepository
{
    private readonly DataContext _dataContext;
    private readonly IGroupRepository _groupRepository;

    public ProductBaseRepository(DataContext context, IGroupRepository groupRepository)
    {
        _dataContext = context;
        _groupRepository = groupRepository;
    }

    // Read
    public async Task<ProductBase> GetProductBaseByIdAsync(int productBaseId)
    {
        var productBase = await _dataContext.ProductBases
            .Include(pb => pb.Categories)
            .Where(pb => pb.ProductBaseId == productBaseId)
            .SingleOrDefaultAsync();

        return productBase;
    }

    public async Task<IEnumerable<ProductBaseDTO>> GetAllProductBasesAsync(ProductBaseSortFilterPageOptions options)
    {
        var productBases = await _dataContext.ProductBases
            .AsNoTracking()
            .Include(pb => pb.Categories)
            .MapProductBaseToDTO()
            .OrderProductBasesBy(options.OrderBy)
            .FilterProductBasesBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToListAsync();
        return productBases;
    }

    public async Task<int> GetProductBasesCountAsync(ProductBaseSortFilterPageOptions options)
    {
        var count = _dataContext.ProductBases
            .AsNoTracking()
            .MapProductBaseToDTO()
            .OrderProductBasesBy(options.OrderBy)
            .FilterProductBasesBy(options.FilterBy, options.FilterValue)
            .Count();
        int countRes = Convert.ToInt32(count);
        return countRes;
    }

    // Create
    public async Task AddProductBaseAsync(ProductBase productBase)
    {
        if (productBase == null)
            throw new ArgumentNullException(nameof(productBase));

        if (productBase.Categories != null)
        {
            var categories = new List<Category>();
            foreach (var category in productBase.Categories)
            {
                var loadedCategory = await _dataContext.Categories.FindAsync(category.CategoryId);
                if (loadedCategory != null)
                    categories.Add(loadedCategory);
            }

            productBase.Categories = categories;
        }

        productBase.ProductBaseId = 0;
        _dataContext.ProductBases.Add(productBase);

        await _dataContext.SaveChangesAsync();
    }

    // Update
    public async Task UpdateProductBaseAsync(ProductBase productBase)
    {
        if (productBase == null)
        {
            throw new ArgumentNullException(nameof(productBase));
        }

        if (productBase.Categories != null)
        {
            var categoriesFromRequest = productBase.Categories.ToList();
            productBase.Categories = null;
            await _dataContext.Entry(productBase).Collection(productBase => productBase.Categories).LoadAsync();
            foreach (var category in categoriesFromRequest)
            {
                if (!productBase.Categories.Any(c => c.CategoryId == category.CategoryId))
                {
                    var loadedCategory = await _dataContext.Categories.FindAsync(category.CategoryId);
                    if (loadedCategory != null)
                        productBase.Categories.Add(loadedCategory);
                }
            }
        }

        _dataContext.Entry(productBase).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateProductBaseAsync(ProductBase productBase, int groupId)
    {
        if (productBase == null)
            return;

        var requesterGroup = await _dataContext.Groups.FindAsync(groupId);
        if (requesterGroup == null)
            return;

        var groupsByProductBaseId = await _groupRepository.GetGroupsByProductBaseIdAsync(productBase.ProductBaseId);

        bool isOtherGroupContainsProductBase = false;
        foreach (var group in groupsByProductBaseId)
            if (group.GroupId != groupId)
            {
                isOtherGroupContainsProductBase = true;
                break;
            }

        if (!isOtherGroupContainsProductBase)
        {
            // Якщо не має залежних груп за записів, то просто змінюємо
            await UpdateProductBaseAsync(productBase);
            return;
        }

        var loadedProductBase = await _dataContext.ProductBases.FindAsync(productBase.ProductBaseId);
        await _dataContext.Entry(loadedProductBase).Collection(pb => pb.Categories).LoadAsync();

        // Якщо є залежні групи, тоді створюємо копію бази та категорій
        var newCategories = new List<Category>();
        foreach (var category in productBase.Categories)
        {
            var existingCategory = await _dataContext.Categories
                .Where(c => c.Title == category.Title && c.Description == category.Description).FirstOrDefaultAsync();
            if (existingCategory != null)
            {
                var loadedCategory = await _dataContext.Categories.FindAsync(existingCategory.CategoryId);
                newCategories.Add(loadedCategory);
                continue;
            }

            var newCategory = new Category
            {
                CategoryId = 0,
                Title = category.Title,
                Description = category.Description,
            };
            newCategories.Add(newCategory);
        }

        var newProductBase = new ProductBase
        {
            ProductBaseId = 0,
            Name = productBase.Name,
            Description = productBase.Description,
            Weight = productBase.Weight,
            RunningOutQuantity = productBase.RunningOutQuantity,
            Categories = newCategories,
        };

        // зберігаємо їх як нові записи
        await _dataContext.ProductBases.AddAsync(newProductBase);
        await _dataContext.SaveChangesAsync();
        // усі попередні записи Product змінюємо на новий запис

        await _dataContext.Products
            .Where(product => product.ProductBaseId == loadedProductBase.ProductBaseId)
            .Include(product => product.Place)
            .ThenInclude(place => place.Location)
            .ThenInclude(location => location.Group)
            .Where(t => t.Place.Location.GroupId == requesterGroup.GroupId)
            .ForEachAsync(product => product.ProductBaseId = newProductBase.ProductBaseId);
        await _dataContext.SaveChangesAsync();
    }

    // Delete
    public async Task DeleteProductBaseAsync(int productBaseId)
    {
        var productBase = await _dataContext.ProductBases.FindAsync(productBaseId);
        if (productBase != null)
        {
            _dataContext.ProductBases.Remove(productBase);
            await _dataContext.SaveChangesAsync();
        }
    }

    public async Task DeleteProductBaseAsync(int productBaseId, int groupId)
    {
        var groupsByProductBaseId = await _groupRepository.GetGroupsByProductBaseIdAsync(productBaseId);

        bool isOtherGroupContainsProductBase = false;
        foreach (var group in groupsByProductBaseId)
            if (group.GroupId != groupId)
            {
                isOtherGroupContainsProductBase = true;
                break;
            }

        if (isOtherGroupContainsProductBase)
            throw new Exception("Other groups depends on this product base");

        // Якщо не має залежних груп за записів, то просто змінюємо
        await DeleteProductBaseAsync(productBaseId);
    }


    public async Task AddCategoryToProductBase(ProductBaseCategoryProp prop)
    {
        var productBase = await _dataContext.ProductBases
            .Include(pb => pb.Categories)
            .Where(pb => pb.ProductBaseId == prop.ProductBaseId)
            .SingleOrDefaultAsync();
        var category = await _dataContext.Categories
            .Where(category => category.CategoryId == prop.CategoryId)
            .SingleOrDefaultAsync();

        if (productBase == null || category == null)
            return;

        productBase.Categories.Add(category);
        await _dataContext.SaveChangesAsync();
    }

    public async Task RemoveCategoryFromProductBase(ProductBaseCategoryProp prop)
    {
        var productBase = await _dataContext.ProductBases
            .Include(pb => pb.Categories)
            .Where(pb => pb.ProductBaseId == prop.ProductBaseId)
            .SingleOrDefaultAsync();
        var category = await _dataContext.Categories
            .Where(category => category.CategoryId == prop.CategoryId)
            .SingleOrDefaultAsync();

        if (productBase == null || category == null)
            return;

        productBase.Categories.Remove(category);
        await _dataContext.SaveChangesAsync();
    }
}