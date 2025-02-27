﻿using DataLayer.Data;
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

    public async Task AddSeedBaseProducts()
    {
        // Seed Categories
        var categories = new List<Category>
        {
            new() { Title = "Electronics", Description = "Devices and gadgets" },
            new() { Title = "Clothing", Description = "Apparel and accessories" },
            new() { Title = "Home Appliances", Description = "Appliances for household use" },
            new() { Title = "Books", Description = "Printed and digital books" },
            new() { Title = "Toys", Description = "Toys and games for children" },
            new() { Title = "Groceries", Description = "Food items and daily essentials" },
            new() { Title = "Personal Care", Description = "Hygiene and beauty products" },
            new() { Title = "Cleaning Supplies", Description = "Household cleaning products" },
            new() { Title = "Eco-Friendly", Description = "Sustainable and environmentally friendly products" }
        };

        await _dataContext.Categories.AddRangeAsync(categories);
        await _dataContext.SaveChangesAsync();

        // Seed ProductBases
        var productBases = new List<ProductBase>
        {
            new()
            {
                Name = "Smartphone",
                Description = "Latest model smartphone with high resolution camera",
                Weight = 0.2,
                RunningOutQuantity = 10,
                Categories = new List<Category> { categories[0] }
            },
            new()
            {
                Name = "T-shirt",
                Description = "Cotton T-shirt available in various colors",
                Weight = 0.3,
                RunningOutQuantity = 50,
                Categories = new List<Category> { categories[1] }
            },
            new()
            {
                Name = "Microwave Oven",
                Description = "Compact microwave oven for quick heating",
                Weight = 12.0,
                RunningOutQuantity = 5,
                Categories = new List<Category> { categories[2] }
            },
            new()
            {
                Name = "Fiction Novel",
                Description = "Best-selling fiction novel",
                Weight = 0.5,
                RunningOutQuantity = 20,
                Categories = new List<Category> { categories[3] }
            },
            new()
            {
                Name = "Lego Set",
                Description = "Creative building blocks for kids",
                Weight = 2.0,
                RunningOutQuantity = 15,
                Categories = new List<Category> { categories[4] }
            },
            new()
            {
                Name = "Milk",
                Description = "Fresh organic milk",
                Weight = 1.0,
                RunningOutQuantity = 30,
                Categories = new List<Category> { categories[5], categories[8] }
            },
            new()
            {
                Name = "Shampoo",
                Description = "Herbal shampoo for daily use",
                Weight = 0.5,
                RunningOutQuantity = 20,
                Categories = new List<Category> { categories[6], categories[8] }
            },
            new()
            {
                Name = "Dish Soap",
                Description = "Eco-friendly dishwashing liquid",
                Weight = 0.8,
                RunningOutQuantity = 15,
                Categories = new List<Category> { categories[7], categories[8] }
            },
            new()
            {
                Name = "Bread",
                Description = "Whole grain bread",
                Weight = 0.5,
                RunningOutQuantity = 25,
                Categories = new List<Category> { categories[5] }
            },
            new()
            {
                Name = "Reusable Grocery Bag",
                Description = "Durable and eco-friendly shopping bag",
                Weight = 0.2,
                RunningOutQuantity = 50,
                Categories = new List<Category> { categories[8] }
            }
        };

        await _dataContext.ProductBases.AddRangeAsync(productBases);
        await _dataContext.SaveChangesAsync();
    }
}