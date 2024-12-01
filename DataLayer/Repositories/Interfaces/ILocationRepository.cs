using Models.DTOs;
using Models.Models;
using Models.Options;

namespace DataLayer.Repositories.Interfaces;

public interface ILocationRepository
{
    Task<Location> AddLocationAsync(Location location);
    Task<List<Location>> AddMockLocationsAsync(int count = 10);
    Task DeleteLocationAsync(int locationId);
    Task<IEnumerable<LocationDTO>> GetAllLocationsAsync(LocationSortFilterPageOptions options);
    Task<IEnumerable<LocationDTO>> GetAllLocationsAsync(LocationSortFilterPageOptions options, int userId);
    Task<Location?> GetLocationAsync(int id, int userId);
    Task<Location> GetLocationByIdAsync(int locationId);
    Task<IEnumerable<Location>> GetLocationsByGroupId(int groupId);
    Task<List<Location>> GetLocationsByUserId(int userId);
    Task<Location> UpdateLocationAsync(Location location);
}