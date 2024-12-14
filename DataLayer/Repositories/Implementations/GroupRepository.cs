using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Models;
using Models.Options;
using Models.Props;
using ServiceLayer.LinqExtensions;
using ServiceLayer.Services;

namespace DataLayer.Repositories.Implementations;

public class GroupRepository : IGroupRepository
{
    private readonly DataContext _dataContext;
    private readonly DataGenerator _dataGenerator;

    public GroupRepository(DataGenerator dataGenerator, DataContext dataContext)
    {
        _dataGenerator = dataGenerator;
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<GroupDTO>> GetGroupsAsync(GroupSortFilterPageOptions options)
    {
        var groups = await _dataContext.Groups
            .AsNoTracking()
            .Include(Group => Group.Users)
            .MapGroupToDTO()
            .OrderGroupsBy(options.OrderBy)
            .FilterGroupsBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToListAsync();
        return groups;
    }

    public async Task<IEnumerable<GroupDTO>> GetGroupsByUserIdAsync(GroupSortFilterPageOptions options, int userId)
    {
        var groups = await _dataContext.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .MapGroupToDTO()
            .OrderGroupsBy(options.OrderBy)
            .FilterGroupsBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToListAsync();
        return groups;
    }

    public async Task<IEnumerable<GroupDTO>> GetGroupsByUserIdAsync(int userId)
    {
        var user = _dataContext.Users.Local.FirstOrDefault(u => u.UserId == userId);

        if (user == null)
        {
            user = await _dataContext.Users
                .Include(u => u.Groups)
                .ThenInclude(g => g.Users)
                .Where(u => u.UserId == userId)
                .FirstOrDefaultAsync();
        }

        if (user != null)
        {
            var groups = user.Groups.AsQueryable()
                .MapGroupToDTO()
                .ToList();
            return groups;
        }

        return Enumerable.Empty<GroupDTO>();
    }

    public async Task<Group?> GetFullGroupAsync(int id)
    {
        var group = await _dataContext.Groups
            .Where(g => g.GroupId == id)!
            .Include(group => group.Users)
            .Include(group => group.Locations)!
            .ThenInclude(location => location.Places)!
            .ThenInclude(place => place.Products)!
            .ThenInclude(product => product.ProductBase)
            .SingleOrDefaultAsync();
        return group;
    }

    public async Task<Group?> GetGroupAsync(int id)
    {
        var group = await _dataContext.Groups
            .Include(g => g.Users)
            .Where(g => g.GroupId == id)
            .SingleOrDefaultAsync();
        return group;
    }

    public async Task<Group> GetGroupForReportAsync(int id)
    {
        var group = await _dataContext.Groups
            .Include(group => group.Users)
            .Include(group => group.Locations)
            .ThenInclude(location => location.Places)
            .ThenInclude(place => place.Products)
            .ThenInclude(product => product.ProductBase)
            .ThenInclude(productBase => productBase.Categories)
            .Include(group => group.ShoppingLists)
            .ThenInclude(shoppingList => shoppingList.ShoppingProducts)
            .ThenInclude(shoppingProduct => shoppingProduct.ProductBase)
            .ThenInclude(productBase => productBase.Categories)
            .Where(group => group.GroupId == id)
            .SingleOrDefaultAsync();
        return group;
    }

    public async Task<Group?> GetGroupAsync(int id, int userId)
    {
        var group = await _dataContext.Groups
            .Include(g => g.Users)
            .Include(g => g.Locations)
            .Where(g => g.GroupId == id)
            .SingleOrDefaultAsync();
        var isUserInGroup = group.Users.Find(user => user.UserId == userId) != null ? true : false;
        if (isUserInGroup)
            return group;
        return null;
    }

    public async Task<Group?> GetGroupByInvintationCode(string invintationCode)
    {
        var groupNoTrack = await _dataContext.Groups
            .AsNoTracking()
            .Where(group => group.InvintationCode == invintationCode)
            .FirstOrDefaultAsync();
        var group = await _dataContext.Groups.FindAsync(groupNoTrack.GroupId);
        await _dataContext.Entry(group).Collection(g => g.Users).LoadAsync();
        await _dataContext.Entry(group).Collection(g => g.Locations).LoadAsync();
        return group;
    }

    public async Task<List<Group>> GetGroupsByProductBaseIdAsync(int productBaseId)
    {
        var groupsByProductBaseId = (from groupTable in _dataContext.Groups
                join location in _dataContext.Locations on groupTable.GroupId equals location.GroupId
                join place in _dataContext.Places on location.LocationId equals place.LocationId
                join product in _dataContext.Products on place.PlaceId equals product.PlaceId
                join productBaseTable in _dataContext.ProductBases on product.ProductBaseId equals productBaseTable
                    .ProductBaseId
                where productBaseTable.ProductBaseId == productBaseId
                select groupTable)
            .ToList();
        return groupsByProductBaseId;
    }

    public async Task<Group?> UpdateGroupAsync(int id, Group group)
    {
        if (group.Users != null)
            group.Users = group.Users.Select(user => _dataContext.Users.Find(user.UserId.Value)).ToList();
        _dataContext.Entry(group).State = EntityState.Modified;
        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!GroupExists(id))
            {
                return null;
            }

            throw;
        }

        return group;
    }

    public async Task<Group> AddGroupAsync(Group group)
    {
        if (group.Users != null || group.Users.Count != 0)
            group.Users = group.Users.Select(user => _dataContext.Users.Find(user.UserId.Value)).ToList();
        group.GroupId = null;
        group.CreationDate = DateTime.UtcNow;
        _dataContext.Groups.Add(group);
        await _dataContext.SaveChangesAsync();
        return group;
    }

    public async Task<IEnumerable<Group>> AddMockGroupsAsync(int numOfGroups = 10)
    {
        var addedGroups = new List<Group>();
        for (int i = 0; i < numOfGroups; i++)
        {
            var createdGroup = _dataGenerator.GenerateGroup();
            addedGroups.Add(createdGroup);
            _dataContext.Add(createdGroup);
        }

        await _dataContext.SaveChangesAsync();
        return addedGroups;
    }

    public async Task DeleteGroupAsync(Group group)
    {
        var loadedGroup = await _dataContext.Groups.FindAsync(group.GroupId.Value);

        if (loadedGroup == null)
            return;

        if (loadedGroup.IsUserGroup)
            return;

        _dataContext.Groups.Remove(group);
        await _dataContext.SaveChangesAsync();
    }
    
    public async Task AddUserToGroupAsync(GroupUserProp prop)
    {
        var user = await _dataContext.Users.FindAsync(prop.UserId);
        var group = await _dataContext.Groups
            .Include(g => g.Users)
            .Where(g => g.GroupId == prop.GroupId)
            .SingleOrDefaultAsync();

        if (group == null || user == null)
            return;

        if (group.Users != null)
            group.Users.Add(user);
        else
            group.Users = new List<User> { user };
        await _dataContext.SaveChangesAsync();
    }
    
    public async Task RemoveUserFromGroupAsync(GroupUserProp prop)
    {
        var user = await _dataContext.Users.FindAsync(prop.UserId);
        var group = await _dataContext.Groups
            .Include(g => g.Users)
            .Where(g => g.GroupId == prop.GroupId)
            .SingleOrDefaultAsync();

        if (group == null || user == null)
            return;

        if (group.Users != null)
            group.Users.Remove(user);

        await _dataContext.SaveChangesAsync();
    }
    
    public async Task<Group> GenerateInvintationCode(int groupId)
    {
        var editedGroup = await _dataContext.Groups.FindAsync(groupId);
        var rnd = new Random();
        string code;

        while (true)
        {
            code = rnd.Next(0, 99999999).ToString();
            if (!await IsInvintationCodeTaken(code))
                break;
        }

        editedGroup.InvintationCode = code;
        _dataContext.Entry(editedGroup).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();

        return editedGroup;
    }
    
    public async Task RemoveUsersFromGroupAsync(int groupId, List<int> users)
    {
        var group = await _dataContext.Groups.FindAsync(groupId);
        await _dataContext.Entry(group).Collection(g => g.Users).LoadAsync();

        List<User> newUsers = new List<User>();
        foreach (var user in users)
        {
            var tempUser = await _dataContext.Users.FindAsync(user);
            newUsers.Add(tempUser);
        }

        if (group.Users != null && newUsers != null)
            foreach (var user in newUsers)
                group.Users.Remove(user);

        await _dataContext.SaveChangesAsync();
    }

    private async Task<bool> IsInvintationCodeTaken(string code)
    {
        return await _dataContext.Groups.AnyAsync(g => g.InvintationCode == code);
    }

    private bool GroupExists(int id)
    {
        return _dataContext.Groups.Any(e => e.GroupId == id);
    }
}