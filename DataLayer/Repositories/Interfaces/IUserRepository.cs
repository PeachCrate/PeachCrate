using Models.DTOs;
using Models.Models;
using Models.Options;
using Models.Props;

namespace DataLayer.Repositories.Interfaces;

public interface IUserRepository
{
    public Task<IEnumerable<UserDTO>> GetUsersAsync(UserSortFilterPageOptions options);
    public Task<User?> GetUserAsync(int id);
    public Task<User> AddUserAsync(User user);
    public Task DeleteUserAsync(User user);
    Task AddGroupToUserAsync(GroupUserProp prop);
    Task RemoveGroupFromUserAsync(GroupUserProp prop);
    Task<User?> GetUserByCredentials(string loginOrEmail);
    Task<User?> UpdateUserAsync(int id, User user);
    Task<IEnumerable<User>> GetUsersAsync();
    Task<IEnumerable<User>> GetUserByGroupId(int groupId);
}