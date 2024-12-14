using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Models.DTOs;
using Models.Models;
using Models.Props;
using ServiceLayer.Services;
using System.IdentityModel.Tokens.Jwt;

namespace DataLayer.Repositories.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly PasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly DataContext _dataContext;
    private readonly DataGenerator _dataGenerator;

    public AuthRepository(
        IUserRepository userRepository,
        IGroupRepository groupRepository,
        PasswordHasher passwordHasher,
        DataContext dataContext,
        IRefreshTokenRepository refreshTokenRepository,
        DataGenerator dataGenerator)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _passwordHasher = passwordHasher;
        _dataContext = dataContext;
        _refreshTokenRepository = refreshTokenRepository;
        _dataGenerator = dataGenerator;
    }

    public async Task<User> RegisterUserAsync(RegisterDTO registerDto)
    {
        string hash, salt;
        (hash, salt) = await _passwordHasher.CreatePasswordHashAsync(registerDto.Password);
        User user = new()
        {
            Login = registerDto.Login!,
            Email = registerDto.Email, 
            PasswordHash = hash, 
            PasswordSalt = salt, 
            RegistrationDate = DateTime.UtcNow
        };

        await _userRepository.AddUserAsync(user);
        Group userGroup = new()
        {
            IsUserGroup = true,
            CreationDate = DateTime.UtcNow,
            Title = user.Login
        };
        await _groupRepository.AddGroupAsync(userGroup);
        return user;
    }

    public async Task<(JwtToken, JwtToken)> LoginAsync(LoginProp prop)
    {
        var userForLogin = await _userRepository.GetUserByCredentials(prop.LoginOrEmail);
        if (userForLogin == null)
        {
            throw new Exception("Bad credentials");
        }

        bool isPasswordRight =
            await _passwordHasher.VerifyPasswordHashAsync(prop.Password, userForLogin.PasswordHash,
                userForLogin.PasswordSalt);
        if (!isPasswordRight)
            throw new Exception("Bad credentials");

        var accessJwtToken = _passwordHasher.GenerateToken(userForLogin);
        var refreshJwtToken = _passwordHasher.GenerateToken(userForLogin, true);

        // save refresh token for user
        var refreshToken = refreshJwtToken.JwtToRefreshToken();

        refreshToken.UserId = userForLogin.UserId.Value;
        await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);

        return (accessJwtToken, refreshJwtToken);
    }

    public async Task<(JwtToken, JwtToken)> RefreshToken(string refreshToken)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);

        var claims = jwt.Claims.ToList();

        var userIdClaim = claims.Where(claim => claim.Type == "UserId").SingleOrDefault();

        var userIdStr = userIdClaim != null ? userIdClaim.Value : null;
        if (!int.TryParse(userIdStr, out var userIdFromJwt))
        {
            throw new Exception("User id in jwt is invalid");
        }

        var userFromJwt = await _userRepository.GetUserAsync(userIdFromJwt);
        if (userFromJwt.RefreshToken.Token != refreshToken)
        {
            throw new Exception("Invalid refresh token.");
        }
        else if (userFromJwt.RefreshToken.Expires < DateTime.Now)
        {
            throw new Exception("Token expired.");
        }

        JwtToken accessJwtToken = _passwordHasher.GenerateToken(userFromJwt);
        JwtToken generatedRefreshJwtToken = _passwordHasher.GenerateToken(userFromJwt, true);

        var generatedRefreshToken = generatedRefreshJwtToken.JwtToRefreshToken();
        generatedRefreshToken.User = userFromJwt;
        generatedRefreshToken.UserId = userFromJwt.UserId.Value;

        await _refreshTokenRepository.AddRefreshTokenAsync(generatedRefreshToken);

        generatedRefreshToken.User.RefreshToken = null;
        return (accessJwtToken, generatedRefreshJwtToken);
    }

    public async Task<IEnumerable<User>> AddMockUsersAsync(int numOfUsers = 10)
    {
        var addedUsers = new List<User>();
        for (int i = 0; i < numOfUsers; i++)
        {
            var createdUser = _dataGenerator.GeneratePerson();
            RegisterDTO registerDto = new(createdUser.Login, createdUser.Email, createdUser.PasswordHash);
            createdUser = await RegisterUserAsync(registerDto);
            addedUsers.Add(createdUser);
        }

        return addedUsers;
    }
}