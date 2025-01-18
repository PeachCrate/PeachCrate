using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Models;
using Models.Options;
using Models.Props;
using ServiceLayer.LinqExtensions;

namespace DataLayer.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly DataContext _dataContext;

    public UserRepository(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<UserDTO>> GetUsersAsync(UserSortFilterPageOptions options)
    {
        var users = await _dataContext.Users
            .AsNoTracking()
            .Include(user => user.Groups)
            .MapUsersToDTO()
            .OrderUsersBy(options.OrderBy)
            .FilterUsersBy(options.FilterBy, options.FilterValue)
            .Page(options.PageStart, options.PageNum)
            .ToListAsync();
        return users;
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        var users = await _dataContext.Users
            .AsNoTracking()
            .Include(user => user.Groups)
            .ToListAsync();
        return users;
    }

    public async Task<User?> GetUserAsync(int id)
    {
        var user = await _dataContext.Users
            .Include(u => u.Groups)
            .Include(u => u.RefreshToken)
            .Where(u => u.UserId == id)
            .SingleOrDefaultAsync();
        return user;
    }

    public async Task<User?> GetUserByCredentials(string loginOrEmail)
    {
        var searchedUser = await _dataContext.Users
            .AsNoTracking()
            .Where(user => user.Login == loginOrEmail || user.Email == loginOrEmail)
            .SingleOrDefaultAsync();
        return searchedUser;
    }

    public async Task<IEnumerable<User>> GetUserByGroupId(int groupId)
    {
        var usersByGroupId = await _dataContext.Users
            .AsNoTracking()
            .Where(user => user.Groups.Any(g => g.GroupId == groupId))
            .ToListAsync();
        return usersByGroupId;
    }

    public async Task<User?> UpdateUserAsync(int id, User user)
    {
        _dataContext.Entry(user).State = EntityState.Modified;
        try
        {
            await _dataContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
            {
                return null;
            }

            throw;
        }

        return user;
    }

    public async Task<User> AddUserAsync(User user)
    {
        if (await IsLoginTakenAsync(user.Login))
            return null;

        var existingUser = await GetUserByCredentials(user.Email);
        if (existingUser is null)
        {
            user.UserId = null;
            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();
            Group userGroup = new()
            {
                IsUserGroup = true,
                CreationDate = DateTime.UtcNow,
                Title = user.Login,
                Users = [user]
            };
            _dataContext.Groups.Add(userGroup);
            await _dataContext.SaveChangesAsync();
            return user;
        }

        existingUser = user;
        _dataContext.Entry(existingUser).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
        return existingUser;
    }
    
    public async Task DeleteUserAsync(User user)
    {
        _dataContext.Users.Remove(user);
        await _dataContext.SaveChangesAsync();
    }

    public async Task AddGroupToUserAsync(GroupUserProp prop)
    {
        var group = await _dataContext.Groups.FindAsync(prop.GroupId);
        var user = await _dataContext.Users
            .Include(u => u.Groups)
            .Where(u => u.UserId == prop.UserId)
            .SingleOrDefaultAsync();

        if (group == null || user == null)
            return;

        user.Groups.Add(group);
        await _dataContext.SaveChangesAsync();
    }

    public async Task RemoveGroupFromUserAsync(GroupUserProp prop)
    {
        var group = await _dataContext.Groups.FindAsync(prop.GroupId);
        var user = await _dataContext.Users
            .Include(u => u.Groups)
            .Where(u => u.UserId == prop.UserId)
            .SingleOrDefaultAsync();

        if (group == null || user == null)
            return;

        user.Groups.Remove(group);
        await _dataContext.SaveChangesAsync();
    }

    private bool UserExists(int id)
    {
        return _dataContext.Users.Any(e => e.UserId == id);
    }

    public async Task<bool> IsLoginTakenAsync(string login)
    {
        return await _dataContext.Users.AnyAsync(u => u.Login == login);
    }
    
    public async Task<bool> IsEmailUsedAsync(string email)
    {
        return await _dataContext.Users.AnyAsync(u => u.Email == email);
    }
}