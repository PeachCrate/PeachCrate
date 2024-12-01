using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using ServiceLayer.Services;

namespace DataLayer.Repositories.Implementations;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DataContext _dataContext;
    private readonly DataGenerator _dataGenerator;


    public RefreshTokenRepository(DataContext context, DataGenerator dataGenerator)
    {
        _dataContext = context;
        _dataGenerator = dataGenerator;
    }

    // Create
    public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
    {
        if (refreshToken == null)
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }
        var savedRefreshToken = await _dataContext.RefreshTokens
            .Where(x => x.UserId == refreshToken.UserId)
            .SingleOrDefaultAsync();

        if (savedRefreshToken == null)
        {
            _dataContext.RefreshTokens.Add(refreshToken);
        }
        else
        {
            savedRefreshToken.Token = refreshToken.Token;
            savedRefreshToken.Created = refreshToken.Created;
            savedRefreshToken.Expires = refreshToken.Expires;
            savedRefreshToken.User = refreshToken.User;
            _dataContext.Entry(savedRefreshToken).State = EntityState.Modified;
        }
        await _dataContext.SaveChangesAsync();
    }

    // Read
    public async Task<RefreshToken> GetRefreshTokenByIdAsync(int refreshTokenId)
    {
        return await _dataContext.RefreshTokens.FindAsync(refreshTokenId);
    }
    
    public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int UserId)
    {
        return await _dataContext.RefreshTokens
            .Where(rt => rt.UserId == UserId)
            .SingleOrDefaultAsync();
    }

    public async Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync()
    {
        return await _dataContext.RefreshTokens.ToListAsync();
    }

    // Update
    public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
    {
        if (refreshToken == null)
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        _dataContext.Entry(refreshToken).State = EntityState.Modified;
        await _dataContext.SaveChangesAsync();
    }

    // Delete
    public async Task DeleteRefreshTokenAsync(int refreshTokenId)
    {
        var refreshToken = await _dataContext.RefreshTokens.FindAsync(refreshTokenId);
        if (refreshToken != null)
        {
            _dataContext.RefreshTokens.Remove(refreshToken);
            await _dataContext.SaveChangesAsync();
        }
    }
}
