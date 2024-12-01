using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class PlaceLinqExtensions
{
    public static IQueryable<PlaceDTO> MapPlacesToDTO(this IQueryable<Place> places)
    {
        return places.Select(place => new PlaceDTO
        {
            PlaceId = place.PlaceId,
            Name = place.Name,
            Description = place.Description,
            Location = place.Location,
            Products = place.Products,
        });
    }

    public static IQueryable<Place> MapDTOToPlaces(this IQueryable<PlaceDTO> placesDTO)
    {
        return placesDTO.Select(placeDTO => new Place
        {
            PlaceId = placeDTO.PlaceId,
            Name = placeDTO.Name,
            Description = placeDTO.Description,
            Location = placeDTO.Location,
            Products = placeDTO.Products,
        });
    }

    public static Place RemoveRecursion(this Place place)
    {
        if (place.Location != null)
        {
            place.Location.Places = null;
        }
        if (place.Products != null)
        {
            place.Products = place.Products.Select(product =>
            {
                product.Place = null;
                product.ProductBase.Products = null;
                return product;
            }).ToList();
        }
        return place;
    }

    public static List<Place> RemoveRecursion(this List<Place> places)
    {
        places.ForEach(place => place.RemoveRecursion());
        return places;
    }


    public static IQueryable<PlaceDTO> OrderPlaceBy(
        this IQueryable<PlaceDTO> places,
        PlaceOrderBy orderByOptions
        )
    {
        switch (orderByOptions)
        {
            case PlaceOrderBy.ByPlaceIdDESC:
                return places.OrderByDescending(place => place.PlaceId);
            case PlaceOrderBy.ByNameDESC:
                return places.OrderByDescending(place => place.Name);
            case PlaceOrderBy.ByPlaceIdASC:
                return places.OrderBy(place => place.PlaceId);
            case PlaceOrderBy.ByNameASC:
                return places.OrderBy(place => place.Name);
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }
    public static IQueryable<PlaceDTO> FilterPlacesBy(
        this IQueryable<PlaceDTO> places,
        PlaceFilterBy filterBy,
        string filterValue)
    {
        if (string.IsNullOrWhiteSpace(filterValue))
            return places;

        switch (filterBy)
        {
            case PlaceFilterBy.NoFilter:
                return places;
            case PlaceFilterBy.ByName:
                return places.Where(x => x.Name.Contains(filterValue));
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(places), filterBy, null);
        }
    }

}
