using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Enums;
using Models.Models;
using Models.Options;
using ServiceLayer.LinqExtensions;
using ServiceLayer.Services;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LocationController : ControllerBase
{
    private readonly ILocationRepository _locationRepository;
    private readonly UserService _userService;

    public LocationController(ILocationRepository locationRepository, UserService userService)
    {
        _locationRepository = locationRepository;
        _userService = userService;
    }

    // GET: api/locations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LocationDTO>>> GetLocations(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0)
    {
        if (!Enum.TryParse(orderBy, out LocationOrderBy orderByOption))
        {
            orderByOption = LocationOrderBy.ByLocationIdASC;
        }

        if (!Enum.TryParse(filterBy, out LocationFilterBy filterByOption))
        {
            filterByOption = LocationFilterBy.NoFilter;
        }

        var options = new LocationSortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        IEnumerable<LocationDTO> locations;
        var userRole = _userService.GetUserRole();
        if (userRole == "User")
        {
            var userId = _userService.GetUserId();
            locations = await _locationRepository.GetAllLocationsAsync(options, userId);
        }
        else
            locations = await _locationRepository.GetAllLocationsAsync(options);
        var noRecursionLocations = locations.AsQueryable().MapDTOToLocations().ToList().RemoveRecursion();
        return Ok(noRecursionLocations);
    }


    [HttpGet("getLocationsByGroupId/{groupId}")]
    public async Task<ActionResult<List<Location>>> GetLocationsByGroupId(int groupId)
    {
        var locations = (await _locationRepository.GetLocationsByGroupId(groupId)).ToList();
        return Ok(locations.RemoveRecursion());
    }

    [HttpGet("getLocationsByUserId/{userId}")]
    public async Task<ActionResult<List<Location>>> GetLocationsByUserId(int userId)
    {
        var locations = (await _locationRepository.GetLocationsByUserId(userId)).ToList();
        return Ok(locations.RemoveRecursion());

    }

    // GET: api/locations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> GetLocation(int id)
    {
        var location = await _locationRepository.GetLocationByIdAsync(id);

        if (location == null)
            return NotFound();

        return Ok(location.RemoveRecursion());
    }

    // POST: api/locations
    [HttpPost]
    public async Task<ActionResult<Location>> PostLocation(Location location)
    {
        await _locationRepository.AddLocationAsync(location);

        return CreatedAtAction("GetLocation", new { id = location.LocationId }, location.RemoveRecursion());
    }

    [HttpPost("addMockLocations")]
    public async Task<ActionResult<List<Location>>> AddMockLocations([FromQuery] int count = 10)
    {
        var locations = await _locationRepository.AddMockLocationsAsync(count);
        return Ok(locations);

    }

    // PUT: api/locations/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLocation(int id, Location location)
    {
        if (id != location.LocationId)
        {
            return BadRequest();
        }

        await _locationRepository.UpdateLocationAsync(location);

        return NoContent();
    }

    // DELETE: api/locations/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        await _locationRepository.DeleteLocationAsync(id);

        return NoContent();
    }


}