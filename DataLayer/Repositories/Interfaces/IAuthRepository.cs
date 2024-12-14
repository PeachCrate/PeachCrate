using Models.DTOs;
using Models.Models;
using Models.Props;

namespace DataLayer.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<IEnumerable<User>> AddMockUsersAsync(int numOfUsers = 10);
    Task<(JwtToken, JwtToken)> LoginAsync(string loginOrEmail, string password);
    Task<User> RegisterUserAsync(string login, string email, string password);
    Task<(JwtToken, JwtToken)> RefreshToken(string refreshToken);
}