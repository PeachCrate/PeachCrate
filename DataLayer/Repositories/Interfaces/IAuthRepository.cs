using Models.DTOs;
using Models.Models;
using Models.Props;

namespace DataLayer.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<(JwtToken, JwtToken)> LoginAsync(string loginOrEmail, string password);
    Task RegisterUserAsync(string login, string email, string password);
    Task<(JwtToken, JwtToken)> RefreshToken(string refreshToken);
}