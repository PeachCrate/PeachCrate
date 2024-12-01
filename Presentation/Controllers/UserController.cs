using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Enums;
using Models.Models;
using Models.Options;
using Models.Props;
using ServiceLayer.Services;


namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly UserService _userService;
    public UserController(IUserRepository userRepository, UserService userService)
    {
        _userRepository = userRepository;
        _userService = userService;
    }

    // GET: api/User
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0)
    {
        if (!Enum.TryParse(orderBy, out UserOrderBy orderByOption))
            orderByOption = UserOrderBy.ByUserIdASC;

        if (!Enum.TryParse(filterBy, out UserFilterBy filterByOption))
            filterByOption = UserFilterBy.NoFilter;

        var options = new UserSortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };
        var users = await _userRepository.GetUsersAsync(options);

        return Ok(users);
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDTO>> GetUser(int id)
    {
        var user = await _userRepository.GetUserAsync(id);
        if (user == null)
            return NotFound();

        return user.UserToDto();
    }

    // PUT: api/User/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(int id, User user)
    {
        if (id != user.UserId)
            return BadRequest();

        var userId = _userService.GetUserId();

        if (user.UserId != userId)
            return Unauthorized();

        await _userRepository.UpdateUserAsync(id, user);

        return NoContent();
    }

    // POST: api/User
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<UserDTO>> PostUser(User user)
    {
        user = await _userRepository.AddUserAsync(user);
        if (user == null)
        {
            return BadRequest("Login taken");
        }
        return CreatedAtAction("GetUser", new { id = user.UserId }, user.UserToDto());
    }
    
    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userRepository.GetUserAsync(id);
        if (user == null)
            return NotFound();

        var userRole = _userService.GetUserRole();
        var userId = _userService.GetUserId();
        if (userRole == "User")
        {
            if (user.UserId != userId)
                return Unauthorized();

            await _userRepository.DeleteUserAsync(user);
        }
        else
        {
            await _userRepository.DeleteUserAsync(user);
        }
        return NoContent();
    }
    [HttpPut("addGroupToUser")]
    public async Task<ActionResult<UserDTO>> AddGroupToUser(GroupUserProp prop)
    {
        await _userRepository.AddGroupToUserAsync(prop);
        var user = await _userRepository.GetUserAsync(prop.UserId);
        return CreatedAtAction("GetUser", new { id = user.UserId }, user.UserToDto());
    }

    [HttpPut("deleteGroupFromUser")]
    public async Task<ActionResult<UserDTO>> RemoveGroupToUser(GroupUserProp prop)
    {
        await _userRepository.RemoveGroupFromUserAsync(prop);
        var user = await _userRepository.GetUserAsync(prop.UserId);
        return CreatedAtAction("GetUser", new { id = user.UserId }, user.UserToDto());
    }
}