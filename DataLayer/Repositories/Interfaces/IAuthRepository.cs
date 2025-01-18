using Models.DTOs;
using Models.Models;
using Models.Props;
using Models.Responses;

namespace DataLayer.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<JwtTokensResponse> LoginAsync(string loginOrEmail, string password);
    Task<JwtTokensResponse> RegisterUserAsync(string login, string email, string password, string? clerkId);
    Task<JwtTokensResponse> OAuthSignInAsync(string sessionId);
    Task<JwtTokensResponse> RefreshToken(string refreshToken);
    Task<bool> IsCredentialTaken(string login, string email);

    Task<bool> DeleteUser();
}