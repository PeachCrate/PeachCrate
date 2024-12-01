using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class LocationLinqExtensions
{
    public static IQueryable<LocationDTO> MapLocationsToDTO(this IQueryable<Location> locations)
    {
        return locations.Select(location => new LocationDTO
        {
            LocationId = location.LocationId,
            Title = location.Title,
            Description = location.Description,
            Group = location.Group,
        });
    }
    public static IQueryable<Location> MapDTOToLocations(this IQueryable<LocationDTO> locationsDTO)
    {
        return locationsDTO.Select(locationDTO => new Location
        {
            LocationId = locationDTO.LocationId,
            Title = locationDTO.Title,
            Description = locationDTO.Description,
            Address = locationDTO.Address,
            Group = locationDTO.Group,
            GroupId = locationDTO.Group.GroupId.Value,
            Places = locationDTO.Places
        });
    }
    public static Location RemoveRecursion(this Location location)
    {
        if (location.Group != null)
        {
            location.Group.Locations = null;
        }
        if (location.Places != null)
        {
            location.Places = location.Places.Select(place =>
            {
                var newPlace = new Place
                {
                    PlaceId = place.PlaceId,
                    Name = place.Name,
                    Description = place.Description,
                    Location = null,
                    Products = place.Products,
                };
                return newPlace;
            }).ToList();
        }
        return location;
    }
    public static List<Location> RemoveRecursion(this List<Location> locations)
    {
        locations.ForEach(location => location.RemoveRecursion());
        return locations;
    }

    public static IQueryable<LocationDTO> OrderLocationBy(
        this IQueryable<LocationDTO> locations,
        LocationOrderBy orderByOptions
        )
    {
        switch (orderByOptions)
        {
            case LocationOrderBy.ByLocationIdDESC:
                return locations.OrderByDescending(Location => Location.LocationId);
            case LocationOrderBy.ByTitleDESC:
                return locations.OrderByDescending(Location => Location.Title);
            case LocationOrderBy.ByAddressDESC:
                return locations.OrderByDescending(Location => Location.Address);
            case LocationOrderBy.ByLocationIdASC:
                return locations.OrderBy(Location => Location.LocationId);
            case LocationOrderBy.ByTitleASC:
                return locations.OrderBy(Location => Location.Title);
            case LocationOrderBy.ByAddressASC:
                return locations.OrderBy(Location => Location.Address);
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }
    public static IQueryable<LocationDTO> FilterLocationsBy(
        this IQueryable<LocationDTO> Locations,
        LocationFilterBy filterBy,
        string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
            return Locations;

        switch (filterBy)
        {
            case LocationFilterBy.NoFilter:
                return Locations;
            case LocationFilterBy.ByTitle:
                return Locations.Where(x => x.Title.Contains(filterValue));
            case LocationFilterBy.ByAddress:
                return Locations.Where(x => x.Address.Contains(filterValue));
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(Locations), filterBy, null);
        }
    }

}
