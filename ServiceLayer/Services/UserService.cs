using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ServiceLayer.Services;
public class UserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private Claim GetClaim(string claimType)
    {
        if (_httpContextAccessor == null) return null;
        return _httpContextAccessor.HttpContext.User.FindFirst(claimType);
    }

    public int GetUserId()
    {
        var userIdClaim = GetClaim("UserId");
        return userIdClaim!= null? Convert.ToInt32(userIdClaim.Value) : 0;
    }

    public string GetUserRole()
    {
        var userRoleClaim = GetClaim(ClaimTypes.Role);
        return userRoleClaim?.Value;
    }
}