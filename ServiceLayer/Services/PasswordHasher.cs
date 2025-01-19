using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ServiceLayer.Services;

public class PasswordHasher
{
    private readonly IConfiguration _configuration;
    public PasswordHasher(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public static string ComputeHash(string password, string salt, string pepper, int iteration)
    {
        if (iteration <= 0) return password;

        using var sha256 = SHA256.Create();
        var passwordSaltPepper = $"{password}{salt}{pepper}";
        var byteValue = Encoding.UTF8.GetBytes(passwordSaltPepper);
        var byteHash = sha256.ComputeHash(byteValue);
        var hash = Convert.ToBase64String(byteHash);
        return ComputeHash(hash, salt, pepper, iteration - 1);
    }

    public static string GenerateSalt()
    {
        using var rng = RandomNumberGenerator.Create();
        var byteSalt = new byte[16];
        rng.GetBytes(byteSalt);
        var salt = Convert.ToBase64String(byteSalt);
        return salt;
    }
    public JwtToken GenerateToken(User user, bool isRefreshToken = false)
    {
        var accessExpirationDate = DateTime.UtcNow.AddMinutes(1);
        var expDate = isRefreshToken ? DateTime.UtcNow.AddDays(31) : DateTime.UtcNow.AddMinutes(1);
        var jwtToken = new JwtToken
        {
            Token = CreateToken(user, expDate, isRefreshToken),
            ExpiresAt = expDate,
            IssuedAt = DateTime.UtcNow
        };

        return jwtToken;
    }
    public string CreateToken(User user, DateTime expirationDate, bool isRefreshToken = false)
    {
        List<Claim> claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.Login),
                new(ClaimTypes.Role, "User"),
                new("UserId", user.UserId!.Value.ToString()),
                new("IsRefreshToken", isRefreshToken.ToString()),
                new("ClerkId", user.ClerkId!),
            };
        var secretKey = _configuration.GetSection("JWT:Token").Value!.ToString();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        
        var token = new JwtSecurityToken(
            claims: claims,
            expires: expirationDate,
            signingCredentials: credentials);

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        return jwtToken;
    }

    public async Task<(string, string)> CreatePasswordHashAsync(string password)
    {
        using var hmac = new HMACSHA512();

        var passwordSalt = hmac.Key;
        var passwordBytes = Encoding.UTF8.GetBytes(password);
        MemoryStream stream = new MemoryStream(passwordBytes);

        var passwordHash = await hmac.ComputeHashAsync(stream);

        var passwordHashString = Convert.ToBase64String(passwordHash);
        var passwordSaltString = Convert.ToBase64String(passwordSalt);

        return (passwordHashString, passwordSaltString);
    }

    public async Task<bool> VerifyPasswordHashAsync(string password, string passwordHash, string passwordSalt)
    {
        byte[] hashBytes = Convert.FromBase64String(passwordHash);
        byte[] saltBytes = Convert.FromBase64String(passwordSalt);

        using var hmac = new HMACSHA512(saltBytes);

        MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(password));
        var computedHash = await hmac.ComputeHashAsync(stream);
        return computedHash.SequenceEqual(hashBytes);
    }
}
