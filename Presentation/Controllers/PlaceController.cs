using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Enums;
using Models.Models;
using Models.Options;
using ServiceLayer.LinqExtensions;

namespace Presentation.Controllers;


[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PlaceController : ControllerBase
{
    private readonly IPlaceRepository _placeRepository;

    public PlaceController(IPlaceRepository placeRepository)
    {
        _placeRepository = placeRepository;
    }

    // GET: api/places
    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PlaceDTO>>> GetPlaces(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0)
    {
        if (!Enum.TryParse(orderBy, out PlaceOrderBy orderByOption))
            orderByOption = PlaceOrderBy.ByPlaceIdASC;

        if (!Enum.TryParse(filterBy, out PlaceFilterBy filterByOption))
            filterByOption = PlaceFilterBy.NoFilter;

        var options = new PlaceSortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        var places = await _placeRepository.GetAllPlacesAsync(options);

        return Ok(places);
    }

    // GET: api/places/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Place>> GetPlace(int id)
    {
        var place = await _placeRepository.GetPlaceByIdAsync(id);
        if (place == null)
            return NotFound();

        return Ok(place.RemoveRecursion());
    }


    [HttpGet("getPlacesByLocationId/{locationId}")]
    public async Task<ActionResult<List<Place>>> GetPlacesByLocationId(int locationId)
    {
        var places = await _placeRepository.GetPlacesByLocationIdAsync(locationId);
        if (places == null)
            return null;

        var placesNoRecursion = places.RemoveRecursion();
        return Ok(placesNoRecursion);
    }


    // POST: api/places
    [HttpPost]
    public async Task<ActionResult<Place>> PostPlace(Place place)
    {
        await _placeRepository.AddPlaceAsync(place);
        return CreatedAtAction("GetPlace", new { id = place.PlaceId }, place.RemoveRecursion());
    }

    // PUT: api/places/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPlace(int id, Place place)
    {
        if (id != place.PlaceId)
            return BadRequest();

        await _placeRepository.UpdatePlaceAsync(place);
        return NoContent();
    }

    // DELETE: api/places/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlace(int id)
    {
        await _placeRepository.DeletePlaceAsync(id);
        return NoContent();
    }
}