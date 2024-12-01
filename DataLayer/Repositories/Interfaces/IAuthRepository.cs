using Models.DTOs;
using Models.Models;
using Models.Props;

namespace DataLayer.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<IEnumerable<User>> AddMockUsersAsync(int numOfUsers = 10);
    Task<(JwtToken, JwtToken)> LoginAsync(LoginProp prop);
    Task<User> RegisterUserAsync(RegisterDTO registerDto);
    Task<(JwtToken, JwtToken)> RefreshToken(string refreshToken);
}