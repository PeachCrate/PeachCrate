using Models.Models;

namespace DataLayer.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    Task AddRefreshTokenAsync(RefreshToken refreshToken);
    Task DeleteRefreshTokenAsync(int refreshTokenId);
    Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync();
    Task<RefreshToken> GetRefreshTokenByIdAsync(int refreshTokenId);
    Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int UserId);
    Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
}