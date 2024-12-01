using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Models;
using Models.Options;
using ServiceLayer.LinqExtensions;

namespace DataLayer.Repositories.Implementations;

public class PlaceRepository : IPlaceRepository
{
    private readonly DataContext _dataContext;

    public PlaceRepository(DataContext context)
    {
        _dataContext = context;
    }

    // Create
    public async Task AddPlaceAsync(Place place)
    {
        if (place == null)
            throw new ArgumentNullException(nameof(place));

        if (place.Location != null)
        {
            place.Location = await _dataContext.Locations.FindAsync(place.LocationId);
        }

        _dataContext.Places.Add(place);
        await _dataContext.SaveChangesAsync();
    }

    // Read
    public async Task<Place> GetPlaceByIdAsync(int placeId)
    {
        var place = await _dataContext.Places.FindAsync(placeId);
        if (place == null)
            return null;
        await _dataContext.Entry(place).Reference(p => p.Location).LoadAsync();
        await _dataContext.Entry(place).Collection(p => p.Products).LoadAsync();
        if (place.Products != null)
            place.Products
                .ForEach(product => _dataContext.Entry(product).Reference(p => p.ProductBase).Load());
        return place;
    }

    public async Task<IEnumerable<PlaceDTO>> GetAllPlacesAsync(PlaceSortFilterPageOptions options)
    {
        var places = await _dataContext.Places
            .AsNoTracking()
            .Include(pb => pb.Location)
            .MapPlacesToDTO()
            .OrderPlaceBy(options.OrderBy)
            .FilterPlacesBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToListAsync();
        return places;
    }

    public async Task<List<Place>> GetPlacesByLocationIdAsync(int locationId)
    {
        var location = await _dataContext.Locations.FindAsync(locationId);
        if (location == null)
            return null;
        await _dataContext.Entry(location).Collection(l => l.Places).LoadAsync();
        foreach (var place in location.Places)
        {
            await _dataContext.Entry(place).Collection(p => p.Products).LoadAsync();
            if (place.Products != null)
                place.Products
                    .ForEach(product =>
                        _dataContext.Entry(product).Reference(p => p.ProductBase).Load()
                    );
        }

        var places = await _dataContext.Places
            .Include(place => place.Products)
            .ThenInclude(product => product.ProductBase)
            .Where(place => place.LocationId == locationId)
            .ToListAsync();
        return places;
    }

    // Update
    public async Task UpdatePlaceAsync(Place place)
    {
        if (place == null)
            throw new ArgumentNullException(nameof(place));

        _dataContext.Entry(place).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    // Delete
    public async Task DeletePlaceAsync(int placeId)
    {
        var place = await _dataContext.Places.FindAsync(placeId);
        if (place == null)
            return;
        
        _dataContext.Places.Remove(place);
        await _dataContext.SaveChangesAsync();
    }
}