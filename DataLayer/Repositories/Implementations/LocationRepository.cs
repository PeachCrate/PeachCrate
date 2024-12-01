using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Models;
using Models.Options;
using ServiceLayer.LinqExtensions;
using ServiceLayer.Services;

namespace DataLayer.Repositories.Implementations;

public class LocationRepository : ILocationRepository
{
    private readonly DataContext _dataContext;
    private readonly DataGenerator _dataGenerator;

    public LocationRepository(DataContext context, DataGenerator dataGenerator)
    {
        _dataContext = context;
        _dataGenerator = dataGenerator;
    }

    // Read
    public async Task<Location> GetLocationByIdAsync(int locationId)
    {
        var location = await _dataContext.Locations.FindAsync(locationId);
        if (location == null)
        {
            return null;
        }

        await _dataContext.Entry(location).Reference(l => l.Group).LoadAsync();
        await _dataContext.Entry(location).Collection(l => l.Places).LoadAsync();
        return location;
    }

    public async Task<Location?> GetLocationAsync(int id, int userId)
    {
        var location = await _dataContext.Locations
            .Include(l => l.Group)
            .ThenInclude(g => g.Users)
            .Where(location => location.LocationId == id)
            .SingleOrDefaultAsync();
        var isUserInGroupOfLocation =
            location.Group.Users.Find(user => user.UserId == userId) != null;
        if (isUserInGroupOfLocation)
            return location;
        return null;
    }

    public async Task<IEnumerable<LocationDTO>> GetAllLocationsAsync(LocationSortFilterPageOptions options)
    {
        var locations = await _dataContext.Locations
            .AsNoTracking()
            .Include(l => l.Group)
            .MapLocationsToDTO()
            .OrderLocationBy(options.OrderBy)
            .FilterLocationsBy(options.FilterBy, options.FilterValue)
            .Page(options.PageNum, options.PageStart)
            .ToListAsync();
        return locations;
    }

    public async Task<IEnumerable<LocationDTO>> GetAllLocationsAsync(LocationSortFilterPageOptions options, int userId)
    {
        var locations = _dataContext.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .SelectMany(g => g.Locations)
            .MapLocationsToDTO()
            .OrderLocationBy(options.OrderBy)
            .FilterLocationsBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToList();
        return locations;
    }

    public async Task<IEnumerable<Location>> GetLocationsByGroupId(int groupId)
    {
        var group = await _dataContext.Groups.FindAsync(groupId);
        if (group == null)
        {
            return null;
        }

        await _dataContext.Entry(group).Collection(g => g.Locations).LoadAsync();
        foreach (var location in group.Locations)
            await _dataContext.Entry(location).Collection(l => l.Places).LoadAsync();
        return group.Locations;
    }

    public async Task<List<Location>> GetLocationsByUserId(int userId)
    {
        var user = await _dataContext.Users.FindAsync(userId);
        if (user == null)
        {
            return null;
        }

        var locations = await _dataContext.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .SelectMany(g => g.Locations)
            .ToListAsync();

        return locations;
    }

    // Create
    public async Task<Location> AddLocationAsync(Location location)
    {
        if (location == null)
        {
            throw new ArgumentNullException(nameof(location));
        }

        var group = await _dataContext.Groups.FindAsync(location.GroupId);
        if (group == null)
        {
            throw new ArgumentException("Group is not found", nameof(location));
        }

        location.Group = group;
        _dataContext.Locations.Add(location);
        await _dataContext.SaveChangesAsync();
        return location;
    }

    public async Task<List<Location>> AddMockLocationsAsync(int count = 10)
    {
        var addedLocations = new List<Location>();
        for (int i = 0; i < count; i++)
        {
            var generatedLocation = _dataGenerator.GenerateLocation();
            addedLocations.Add(generatedLocation);
            _dataContext.Add(generatedLocation);
        }

        await _dataContext.SaveChangesAsync();
        return addedLocations;
    }

    // Update
    public async Task<Location> UpdateLocationAsync(Location location)
    {
        if (location == null)
        {
            throw new ArgumentNullException(nameof(location));
        }

        _dataContext.Entry(location).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
        return location;
    }

    // Delete
    public async Task DeleteLocationAsync(int locationId)
    {
        var location = await _dataContext.Locations.FindAsync(locationId);
        if (location != null)
        {
            _dataContext.Locations.Remove(location);
            await _dataContext.SaveChangesAsync();
        }
    }
}