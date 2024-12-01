using DataLayer.Handlers;
using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Enums;
using Models.Models;
using Models.Options;
using Models.Props;
using ServiceLayer.LinqExtensions;
using ServiceLayer.Services;

namespace Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupRepository _groupRepository;
    private readonly UserService _userService;
    private readonly ExcelHelper _excelHelper;

    public GroupController(IGroupRepository groupRepository, UserService userService, ExcelHelper excelHelper)
    {
        _groupRepository = groupRepository;
        _userService = userService;
        _excelHelper = excelHelper;
    }

    // GET: api/Group
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<GroupDTO>>> GetGroups(
        [FromQuery] string? orderBy,
        [FromQuery] string? filterBy,
        [FromQuery] string? filterValue,
        [FromQuery] int pageNum = 10,
        [FromQuery] int pageStart = 0
    )
    {
        if (!Enum.TryParse(orderBy, out GroupOrderBy orderByOption))
        {
            orderByOption = GroupOrderBy.ByGroupIdASC;
        }

        if (!Enum.TryParse(filterBy, out GroupFilterBy filterByOption))
        {
            filterByOption = GroupFilterBy.NoFilter;
        }

        var options = new GroupSortFilterPageOptions()
        {
            OrderBy = orderByOption,
            FilterBy = filterByOption,
            FilterValue = filterValue,
            PageNum = pageNum,
            PageStart = pageStart
        };

        IEnumerable<GroupDTO> groups;
        var userRole = _userService.GetUserRole();
        if (userRole == "User")
        {
            var userId = _userService.GetUserId();
            groups = await _groupRepository.GetGroupsByUserIdAsync(options, userId);
        }
        else
            groups = await _groupRepository.GetGroupsAsync(options);

        return Ok(groups);
    }

    // GET: api/Group/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> GetGroup(int id)
    {
        var group = await _groupRepository.GetGroupAsync(id);

        if (group == null)
        {
            return NotFound();
        }

        var returnGroup = group.RemoveRecursion();
        //return group.GroupToDto();
        return returnGroup;
    }

    [HttpGet("getGroupsByUserId/{userId}")]
    public async Task<ActionResult<List<Group>>> GetGroupsByUserID(int userId)
    {
        var groupsDTO = await _groupRepository.GetGroupsByUserIdAsync(userId);
        var groups = groupsDTO.MapDTOToGroups().AsQueryable().ToList();
        return Ok(groups.RemoveRecursion());
    }

    [HttpGet("getGroupByInvintationCode/{invintationCode}")]
    public async Task<ActionResult<Group>> GetGroupByInvintationCode(string invintationCode)
    {
        var group = await _groupRepository.GetGroupByInvintationCode(invintationCode);
        return Ok(group.RemoveRecursion());
    }

    // POST: api/Group
    [HttpPost]
    public async Task<ActionResult<Group>> PostGroup(Group group)
    {
        group = await _groupRepository.AddGroupAsync(group);

        return Ok(group.RemoveRecursion());
    }

    [HttpPost("addMockGroups")]
    public async Task<ActionResult<Group>> PostMockGroups([FromQuery] int numOfGroups)
    {
        var groups = await _groupRepository.AddMockGroupsAsync(numOfGroups);
        var groupDtos = await groups.AsQueryable().MapGroupToDTO().ToListAsync();
        return CreatedAtAction("GetGroups", groupDtos);
    }

    // PUT: api/Group/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGroup(int id, Group group)
    {
        if (id != group.GroupId)
        {
            return BadRequest();
        }

        await _groupRepository.UpdateGroupAsync(id, group);

        return NoContent();
    }
    
    [HttpPut("addUserToGroup")]
    public async Task<IActionResult> AddUserToGroup(GroupUserProp prop)
    {
        await _groupRepository.AddUserToGroupAsync(prop);
        return NoContent();
    }
    
    [HttpPut("deleteUserFromGroup")]
    public async Task<IActionResult> RemoveUserToGroup(GroupUserProp prop)
    {
        await _groupRepository.RemoveUserFromGroupAsync(prop);
        return NoContent();
    }
    
    [HttpPut("generateInvintationCode/{groupId}")]
    public async Task<IActionResult> GenerateInvintationCode(int groupId)
    {
        var group = await _groupRepository.GetGroupAsync(groupId);
        if (group == null)
            return BadRequest("Group not found!");
        await _groupRepository.GenerateInvintationCode(groupId);
        return NoContent();
    }
    
    [HttpPut("removeUsersFromGroup/{groupId}")]
    public async Task<IActionResult> RemoveUsersFromGroup(int groupId, List<int> usersId)
    {
        var group = await _groupRepository.GetGroupAsync(groupId);
        if (group == null)
        {
            return BadRequest("Group not found");
        }

        await _groupRepository.RemoveUsersFromGroupAsync(groupId, usersId);
        return NoContent();
    }
    
    // DELETE: api/Group/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGroup(int id)
    {
        var group = await _groupRepository.GetGroupAsync(id);
        if (group == null)
        {
            return NotFound();
        }

        await _groupRepository.DeleteGroupAsync(group);
        return NoContent();
    }

    [HttpGet("excelReport/{groupId}")]
    public async Task<IActionResult> GetExcelReport(int groupId)
    {
        var (file, fileName) = await _excelHelper.GetExcelForGroup(groupId);
        return File(file, "application/vnd.ms-excel", fileName);
    }

    [HttpGet("beautifulExcelReport/{groupId}")]
    public async Task<IActionResult> GetBeautifulExcelReport(int groupId)
    {
        var (file, fileName) = await _excelHelper.GetBeautifulExcelReportForGroup(groupId);
        return File(file, "application/vnd.ms-excel", fileName);
    }
}