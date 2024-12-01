
using Models.DTOs;
using Models.Models;

namespace ServiceLayer.Services;

public static class Mapper
{


    public static UserDTO UserToDto(this User user)
    {
        return new UserDTO
        {
            UserId = user.UserId!,
            Login = user.Login,
            PasswordHash = user.PasswordHash,
            PasswordSalt = user.PasswordSalt,
            Password = user.PasswordHash,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            RegistrationDate = user.RegistrationDate,
            Groups = user.Groups != null ? user.Groups
                .Select(g => new Group()
                {
                    GroupId = g.GroupId,
                    Title = g.Title,
                    Description = g.Description,
                    CreationDate = g.CreationDate,
                    InvintationCode = g.InvintationCode
                })
                .ToList()
            : null,
        };
    }

    public static User DTOToUser(this UserDTO userDTO)
    {

        return new User
        {
            UserId = userDTO.UserId,
            Login = userDTO.Login,
            PhoneNumber = userDTO.PhoneNumber,
            Email = userDTO.Email,
            RegistrationDate = userDTO.RegistrationDate,
            Groups = userDTO.Groups != null ? userDTO.Groups
                .Select(g => new Group()
                {
                    GroupId = g.GroupId,
                    Title = g.Title,
                    Description = g.Description,
                    CreationDate = g.CreationDate,
                    InvintationCode = g.InvintationCode
                })
                .ToList()
                : null
        };
    }

    public static GroupDTO GroupToDto(this Group? group)
    {
        return new GroupDTO
        {
            GroupId = group.GroupId,
            Title = group.Title,
            Description = group.Description,
            CreationDate = group.CreationDate,
            InvintationCode = group.InvintationCode,
            Users = group.Users.Select(u => new User()
            {
                UserId = u.UserId,
                Login = u.Login,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
                RegistrationDate = u.RegistrationDate,
            })
            .ToList(),
        };
    }

    public static CategoryDTO CategoryToDTO(this Category? category)
    {
        return new CategoryDTO
        {
            CategoryId = category.CategoryId,
            Title = category.Title,
            Description = category.Description,
            ProductBases = category.ProductBases
        };
    }

    public static LocationDTO LocationToDTO(this Location? location)
    {
        return new LocationDTO
        {
            LocationId = location.LocationId,
            Title = location.Title,
            Description = location.Description,
            Address = location.Address,
            Group = location.Group
        };
    }

    public static Product DTOToProduct(this ProductDTO productDTO)
    {
        return new Product
        {
            ProductId = productDTO.ProductId,
            ProductBaseId = productDTO.ProductBaseId,
            ProductBase = productDTO.ProductBase,
            Quantity = productDTO.Quantity,
            Price = productDTO.Price,
            PlaceId = productDTO.PlaceId,
            Place = productDTO.Place,
            PurchaseDate = productDTO.PurchaseDate,
            ValidUntil = productDTO.ValidUntil,
        };
    }

    public static RefreshToken JwtToRefreshToken(this JwtToken jwtToken)
    {
        return new RefreshToken
        {
            Token = jwtToken.Token,
            Created = jwtToken.IssuedAt,
            Expires = jwtToken.ExpiresAt,
        };
    }
    public static JwtToken RefreshToJwtToken(this RefreshToken refreshToken)
    {
        return new JwtToken
        {
            Token = refreshToken.Token,
            IssuedAt = refreshToken.Created,
            ExpiresAt = refreshToken.Expires,
        };
    }

}
