using Models.DTOs;
using Models.Models;
using Models.Options;


namespace DataLayer.Repositories.Interfaces;

public interface IPlaceRepository
{
    Task AddPlaceAsync(Place place);
    Task DeletePlaceAsync(int placeId);
    Task<IEnumerable<PlaceDTO>> GetAllPlacesAsync(PlaceSortFilterPageOptions options);
    Task<Place> GetPlaceByIdAsync(int placeId);
    Task<List<Place>> GetPlacesByLocationIdAsync(int locationId);
    Task UpdatePlaceAsync(Place place);
}