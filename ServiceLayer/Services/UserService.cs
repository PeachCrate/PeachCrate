using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Models.Models;

namespace ServiceLayer.Services;

public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private Claim? GetClaim(string claimType)
    {
        return _httpContextAccessor.HttpContext.User.FindFirst(claimType);
    }

    public int GetUserId()
    {
        var userIdClaim = GetClaim("UserId");
        return Convert.ToInt32(userIdClaim?.Value);
    }

    public string? GetUserRole()
    {
        var userRoleClaim = GetClaim(ClaimTypes.Role);
        return userRoleClaim?.Value;
    }

    public int GetUserIdFromClaims(List<Claim> claims)
    {
        var claim = claims.FirstOrDefault(c => c.Type == "UserId");
        if (claim is null)
            throw new Exception($"{nameof(claims)} does not contatins claim with type 'UserId'");

        if (!int.TryParse(claim.Value, out int userId))
        {
            throw new AuthException(AuthErrorType.InvalidUserIdInJwtClaims);
        }
        
        return userId;
    }
}