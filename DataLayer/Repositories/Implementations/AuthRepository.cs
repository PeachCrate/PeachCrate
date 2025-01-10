﻿using DataLayer.Data;
using DataLayer.Repositories.Interfaces;
using Models.DTOs;
using Models.Models;
using Models.Props;
using ServiceLayer.Services;
using System.IdentityModel.Tokens.Jwt;
using Models.Responses;

namespace DataLayer.Repositories.Implementations;

public class AuthRepository : IAuthRepository
{
    private readonly PasswordHasher _passwordHasher;
    private readonly IUserRepository _userRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly DataContext _dataContext;
    private readonly DataGenerator _dataGenerator;
    private readonly UserService _userService;

    public AuthRepository(
        IUserRepository userRepository,
        IGroupRepository groupRepository,
        PasswordHasher passwordHasher,
        DataContext dataContext,
        IRefreshTokenRepository refreshTokenRepository,
        DataGenerator dataGenerator,
        UserService userService)
    {
        _userRepository = userRepository;
        _groupRepository = groupRepository;
        _passwordHasher = passwordHasher;
        _dataContext = dataContext;
        _refreshTokenRepository = refreshTokenRepository;
        _dataGenerator = dataGenerator;
        _userService = userService;
    }

    public async Task<JwtTokensResponse> RegisterUserAsync(string login, string email, string password, string? clerkId)
    {
        if (await _userRepository.IsLoginTakenAsync(login))
            throw new AuthException(AuthErrorType.LoginTaken);

        if (await _userRepository.IsEmailUsedAsync(email))
            throw new AuthException(AuthErrorType.EmailIsUsed);
        
        var (hash, salt) = await _passwordHasher.CreatePasswordHashAsync(password);
        User user = new()
        {
            Login = login,
            Email = email,
            PasswordHash = hash,
            PasswordSalt = salt,
            RegistrationDate = DateTime.UtcNow,
            ClerkId = clerkId,
        };

        await _userRepository.AddUserAsync(user);
        Group userGroup = new()
        {
            IsUserGroup = true,
            CreationDate = DateTime.UtcNow,
            Title = user.Login,
            Users = [user]
        };
        await _groupRepository.AddGroupAsync(userGroup);
        return await GenerateTokens(user);
    }

    public async Task<JwtTokensResponse> LoginAsync(string loginOrEmail, string password)
    {
        var userForLogin = await _userRepository.GetUserByCredentials(loginOrEmail);
        if (userForLogin is null)
            throw new AuthException(AuthErrorType.BadCredentials);

        var isPasswordRight =
            await _passwordHasher.VerifyPasswordHashAsync(password, userForLogin.PasswordHash,
                userForLogin.PasswordSalt);
        if (!isPasswordRight)
            throw new AuthException(AuthErrorType.BadCredentials);

        return await GenerateTokens(userForLogin);
    }

    private async Task<JwtTokensResponse> GenerateTokens(User userForLogin)
    {
        var accessJwtToken = _passwordHasher.GenerateToken(userForLogin);
        var refreshJwtToken = _passwordHasher.GenerateToken(userForLogin, true);

        // save refresh token for user
        var refreshToken = refreshJwtToken.JwtToRefreshToken();
        refreshToken.UserId = userForLogin.UserId!.Value;
        await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);
        return new JwtTokensResponse(accessJwtToken, refreshJwtToken);
    }

    public async Task<JwtTokensResponse> RefreshToken(string refreshToken)
    {
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
        var claims = jwt.Claims.ToList();
        var userId = _userService.GetUserIdFromClaims(claims);

        var userFromJwt = await _userRepository.GetUserAsync(userId);
        if (userFromJwt is null)
        {
            throw new AuthException(AuthErrorType.UserNotFound);
        }

        if (userFromJwt.RefreshToken?.Token != refreshToken)
        {
            throw new AuthException(AuthErrorType.InvalidToken);
        }

        if (userFromJwt.RefreshToken.Expires < DateTime.UtcNow)
        {
            throw new AuthException(AuthErrorType.TokenExpired);
        }

        JwtToken accessJwtToken = _passwordHasher.GenerateToken(userFromJwt);
        JwtToken generatedRefreshJwtToken = _passwordHasher.GenerateToken(userFromJwt, true);

        var generatedRefreshToken = generatedRefreshJwtToken.JwtToRefreshToken();
        generatedRefreshToken.User = userFromJwt;
        generatedRefreshToken.UserId = userFromJwt.UserId.Value;

        await _refreshTokenRepository.AddRefreshTokenAsync(generatedRefreshToken);

        generatedRefreshToken.User.RefreshToken = null;
        return new JwtTokensResponse(accessJwtToken, generatedRefreshJwtToken);
    }

    public async Task<bool> IsCredentialTaken(string login, string email)
    {
        if (await _userRepository.IsLoginTakenAsync(login))
            throw new AuthException(AuthErrorType.LoginTaken);

        if (await _userRepository.IsEmailUsedAsync(email))
            throw new AuthException(AuthErrorType.EmailIsUsed);

        return true;
    }
}