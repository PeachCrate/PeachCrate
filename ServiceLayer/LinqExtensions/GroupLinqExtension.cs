using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class GroupLinqExtension
{
    public static IQueryable<GroupDTO> MapGroupToDTO(this IQueryable<Group> Groups)
    {
        return Groups.Select(Group => new GroupDTO
        {
            GroupId = (int)Group.GroupId!,
            Title = Group.Title,
            InvintationCode = Group.InvintationCode,
            Description = Group.Description,
            CreationDate = Group.CreationDate,
            IsUserGroup = Group.IsUserGroup,
            Users = Group.Users
                .Select(user => new User
                {
                    UserId = user.UserId,
                    Login = user.Login,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    RegistrationDate = user.RegistrationDate,
                })
                .ToList()
        });
    }
    public static Group RemoveRecursion(this Group group)
    {
        if (group.Users != null)
            group.Users = group.Users
                .Select(user => { user.Groups = null; return user; })
                .ToList();

        if (group.Locations != null)
            group.Locations = group.Locations
                .Select(locaton =>
                {
                    locaton.Group = null;
                    if (locaton.Places != null)
                        locaton.Places = locaton.Places.Select(p =>
                        {
                            p.Location = null;
                            p.Products = null;
                            return p;
                        })
                        .ToList();
                    return locaton;
                })
                .ToList();



        return group;
    }
    public static List<Group> RemoveRecursion(this List<Group> groups)
    {
        groups.ForEach((group) => group.RemoveRecursion());
        return groups;
    }
    public static IQueryable<Group> MapDTOToGroups(this IQueryable<GroupDTO> Groups)
    {
        return Groups.Select(Group => new Group
        {
            GroupId = (int)Group.GroupId!,
            Title = Group.Title,
            Description = Group.Description,
            InvintationCode = Group.InvintationCode,
            CreationDate = Group.CreationDate,
            IsUserGroup = Group.IsUserGroup,
            Users = Group.Users
                .Select(user => new User
                {
                    UserId = user.UserId,
                    Login = user.Login,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    RegistrationDate = user.RegistrationDate,
                })
                .ToList()
        });
    }
    public static IEnumerable<Group> MapDTOToGroups(this IEnumerable<GroupDTO> Groups)
    {
        return Groups.Select(Group => new Group
        {
            GroupId = (int)Group.GroupId!,
            Title = Group.Title,
            Description = Group.Description,
            InvintationCode = Group.InvintationCode,
            CreationDate = Group.CreationDate,
            IsUserGroup = Group.IsUserGroup,
            Users = Group.Users
                .Select(user => new User
                {
                    UserId = user.UserId,
                    Login = user.Login,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    RegistrationDate = user.RegistrationDate,
                })
                .ToList()
        });
    }

    public static IQueryable<GroupDTO> OrderGroupsBy(this IQueryable<GroupDTO> Groups, GroupOrderBy orderByOptions)
    {
        switch (orderByOptions)
        {
            case GroupOrderBy.ByGroupIdDESC:
                return Groups.OrderByDescending(x => x.GroupId);
            case GroupOrderBy.ByTitleDESC:
                return Groups.OrderByDescending(x => x.Title);
            case GroupOrderBy.ByDescriptionDESC:
                return Groups.OrderByDescending(x => x.Description);
            case GroupOrderBy.ByCreationDateDESC:
                return Groups.OrderByDescending(x => x.CreationDate);
            case GroupOrderBy.ByGroupIdASC:
                return Groups.OrderBy(x => x.GroupId);
            case GroupOrderBy.ByTitleASC:
                return Groups.OrderBy(x => x.Title);
            case GroupOrderBy.ByDescriptionASC:
                return Groups.OrderBy(x => x.Description);
            case GroupOrderBy.ByCreationDateASC:
                return Groups.OrderBy(x => x.CreationDate);
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }



    public static IQueryable<GroupDTO> FilterGroupsBy(this IQueryable<GroupDTO> Groups,
        GroupFilterBy filterBy,
        string filterValue)
    {
        if (string.IsNullOrEmpty(filterValue))
            return Groups;

        switch (filterBy)
        {
            case GroupFilterBy.NoFilter:
                return Groups;
            case GroupFilterBy.ByTitle:
                return Groups.Where(x => x.Title.Contains(filterValue));
            case GroupFilterBy.ByDescription:
                return Groups.Where(x => x.Description.Contains(filterValue));
            case GroupFilterBy.ByCreationDate:
                var filterDate = DateTime.Parse(filterValue);
                return Groups.Where(x => x.CreationDate > filterDate);


            default:
                throw new ArgumentOutOfRangeException(
                    nameof(Groups), filterBy, null);
        }
    }
}