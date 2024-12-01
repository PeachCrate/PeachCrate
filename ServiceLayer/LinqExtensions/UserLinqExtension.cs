using Models.DTOs;
using Models.Enums;
using Models.Models;

namespace ServiceLayer.LinqExtensions;

public static class UserLinqExtension
{
    public static IQueryable<UserDTO> MapUsersToDTO(this IQueryable<User> users)
    {
        return users.Select(user => new UserDTO
        {
            UserId = (int)user.UserId!,
            Login = user.Login,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            RegistrationDate = user.RegistrationDate,
            Groups = user.Groups
                .Select(g => new Group()
                {
                    GroupId = g.GroupId,
                    Title = g.Title,
                    Description = g.Description,
                    CreationDate = g.CreationDate,
                    InvintationCode = g.InvintationCode
                })
                .ToList(),
        });
    }



    public static IQueryable<UserDTO> OrderUsersBy(this IQueryable<UserDTO> users,
        UserOrderBy orderByOptions)
    {
        switch (orderByOptions)
        {
            case UserOrderBy.ByUserIdDESC:
                return users.OrderByDescending(x => x.UserId);
            case UserOrderBy.ByLoginDESC:
                return users.OrderByDescending(x => x.Login);
            case UserOrderBy.ByEmailDESC:
                return users.OrderByDescending(x => x.Email);
            case UserOrderBy.ByRegistrationDateDESC:
                return users.OrderByDescending(x => x.RegistrationDate);
            case UserOrderBy.ByUserIdASC:
                return users.OrderBy(x => x.UserId);
            case UserOrderBy.ByLoginASC:
                return users.OrderBy(x => x.Login);
            case UserOrderBy.ByEmailASC:
                return users.OrderBy(x => x.Email);
            case UserOrderBy.ByRegistrationDateASC:
                return users.OrderBy(x => x.RegistrationDate);
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(orderByOptions), orderByOptions, null);
        }
    }



    public static IQueryable<UserDTO> FilterUsersBy(this IQueryable<UserDTO> users,
        UserFilterBy filterBy,
        string filterValue)
    {
        if (string.IsNullOrEmpty(filterValue))
            return users;

        switch (filterBy)
        {
            case UserFilterBy.NoFilter:
                return users;
            case UserFilterBy.ByLogin:
                return users.Where(x => x.Login.Contains(filterValue));
            case UserFilterBy.ByEmail:
                return users.Where(x => x.Email.Contains(filterValue));
            case UserFilterBy.ByRegistrationDateBefore:
                var filterDate = DateTime.Parse(filterValue);
                return users.Where(x => x.RegistrationDate < filterDate);
            case UserFilterBy.ByRegistrationDateAfter:
                var filterDateAfter = DateTime.Parse(filterValue);
                return users.Where(x => x.RegistrationDate > filterDateAfter);


            default:
                throw new ArgumentOutOfRangeException(
                    nameof(users), filterBy, null);
        }
    }



}