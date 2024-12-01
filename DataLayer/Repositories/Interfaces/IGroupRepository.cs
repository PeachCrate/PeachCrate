using Models.DTOs;
using Models.Models;
using Models.Options;
using Models.Props;

namespace DataLayer.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<Group> AddGroupAsync(Group group);
    Task<IEnumerable<Group>> AddMockGroupsAsync(int numOfGroups = 10);
    Task AddUserToGroupAsync(GroupUserProp prop);
    Task DeleteGroupAsync(Group group);
    Task<Group> GenerateInvintationCode(int groupId);
    Task<Group?> GetGroupAsync(int id);
    Task<Group?> GetGroupAsync(int id, int userId);
    Task<Group?> GetGroupByInvintationCode(string invintationCode);
    Task<IEnumerable<GroupDTO>> GetGroupsAsync(GroupSortFilterPageOptions options);
    Task<IEnumerable<GroupDTO>> GetGroupsByUserIdAsync(GroupSortFilterPageOptions options, int userId);
    Task<IEnumerable<GroupDTO>> GetGroupsByUserIdAsync(int userId);
    Task RemoveUserFromGroupAsync(GroupUserProp prop);
    Task<Group?> UpdateGroupAsync(int id, Group group);
    Task RemoveUsersFromGroupAsync(int groupId, List<int> usersId);
    Task<List<Group>> GetGroupsByProductBaseIdAsync(int productBaseId);
    Task<Group?> GetFullGroupAsync(int id);
    Task<Group> GetGroupForReportAsync(int id);
}
