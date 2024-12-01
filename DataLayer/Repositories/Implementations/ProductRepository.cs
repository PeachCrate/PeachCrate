using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Models;
using Models.Options;
using ServiceLayer.LinqExtensions;
using ServiceLayer.Services;

namespace DataLayer.Repositories.Implementations;

public class ProductRepository : IProductRepository
{
    private readonly DataContext _dataContext;
    private readonly IProductBaseRepository _productBaseRepository;
    private readonly IPlaceRepository _placeRepository;

    public ProductRepository(DataContext context, IProductBaseRepository productBaseRepository,
        IPlaceRepository placeRepository)
    {
        _dataContext = context;
        _productBaseRepository = productBaseRepository;
        _placeRepository = placeRepository;
    }

    // Read
    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _dataContext.Products.FindAsync(productId);
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(ProductSortFilterPageOptions options)
    {
        var products = await _dataContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBase)
            .Include(x => x.Place)
            .Include(x => x.ProductBase.Categories)
            .MapProductToDTO()
            .OrderProductsBy(options.OrderBy)
            .FilterProductsBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToListAsync();
        return products;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync(
        ProductSortFilterPageOptions options,
        int groupId
    )
    {
        var products = await _dataContext.Products
            .AsNoTracking()
            .Include(x => x.ProductBase)
            .Include(x => x.Place)
            .ThenInclude(x => x.Location)
            .ThenInclude(x => x.Group)
            .Include(x => x.ProductBase.Categories)
            .Where(p => p.Place.Location.Group.GroupId.Value == groupId)
            .MapProductToDTO()
            .OrderProductsBy(options.OrderBy)
            .FilterProductsBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToListAsync();
        return products;
    }

    public async Task<int> GetProductCountAsync(
        ProductSortFilterPageOptions options
    )
    {
        var count = _dataContext.Products
            .AsNoTracking()
            .MapProductToDTO()
            .OrderProductsBy(options.OrderBy)
            .FilterProductsBy(options.FilterBy, options.FilterValue)
            .Count();
        int countRes = Convert.ToInt32(count);
        return countRes;
    }

    public async Task<IEnumerable<Product>> GetProductsByPlaceIdAsync(int placeId)
    {
        var place = await _dataContext.Places.FindAsync(placeId);
        if (place == null)
            return null;

        await _dataContext.Entry(place).Collection(p => p.Products).LoadAsync();
        var products = place.Products;

        products.ForEach(product => _dataContext.Entry(product).Reference(p => p.ProductBase).Load());
        return products;
    }


    public async Task<List<Product>> GetProductsByValidToDate(int groupId, DateTime validToDate)
    {
        var productsList = (from products in _dataContext.Products
                join places in _dataContext.Places on products.PlaceId equals places.PlaceId
                join locations in _dataContext.Locations on places.LocationId equals locations.LocationId
                join groups in _dataContext.Groups on locations.GroupId equals groups.GroupId
                where groups.GroupId == groupId && products.ValidUntil < validToDate
                orderby products.ValidUntil
                select products
            ).ToList();
        productsList.ForEach(async p =>
        {
            await _dataContext.Entry(p).Reference(p => p.ProductBase).LoadAsync();
            await _dataContext.Entry(p).Reference(p => p.Place).LoadAsync();
            await _dataContext.Entry(p.Place).Reference(p => p.Location).LoadAsync();
        });
        return productsList;
    }


    // Create
    public async Task<Product> AddProductAsync(ProductDTO productDTO)
    {
        if (productDTO == null)
            throw new ArgumentNullException(nameof(productDTO));

        var productBase = await _productBaseRepository.GetProductBaseByIdAsync(productDTO.ProductBase.ProductBaseId);
        productDTO.ProductBase = productBase;

        var place = await _placeRepository.GetPlaceByIdAsync(productDTO.PlaceId);
        productDTO.Place = place;

        var product = productDTO.DTOToProduct();

        _dataContext.Products.Add(product);
        await _dataContext.SaveChangesAsync();
        return product;
    }

    // Update
    public async Task UpdateProductAsync(Product product)
    {
        if (product == null)
        {
            throw new ArgumentNullException(nameof(product));
        }

        _dataContext.Entry(product).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    // Delete
    public async Task DeleteProductAsync(int productId)
    {
        var product = await _dataContext.Products.FindAsync(productId);
        if (product != null)
        {
            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
        }
    }
}