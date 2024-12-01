using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Models;
using Models.Props;
using ServiceLayer.Services;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterDTO registerDto)
        {
            var user = await _authRepository.RegisterUserAsync(registerDto);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtToken>> Login(LoginProp prop)
        {
            try
            {
                var (accessJwtToken, refreshJwtToken) = await _authRepository.LoginAsync(prop);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = refreshJwtToken.ExpiresAt
                };
                Response.Cookies.Append("refreshToken", refreshJwtToken.Token, cookieOptions);
                return accessJwtToken;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<AuthContoller>
        [HttpPost("refreshToken")]
        public async Task<ActionResult<JwtToken>> RefreshToken()
        {
            var refreshToken = Request.Headers.AsEnumerable().Where(x => x.Key == "refreshToken").Select(x => x.Value)
                .FirstOrDefault().FirstOrDefault();

            try
            {
                JwtToken accessJwtToken;
                JwtToken newRefreshJwtToken;
                (accessJwtToken, newRefreshJwtToken) = await _authRepository.RefreshToken(refreshToken);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = newRefreshJwtToken.ExpiresAt
                };
                Response.Cookies.Append("refreshToken", newRefreshJwtToken.Token, cookieOptions);
                Response.Cookies.Append("accessToken", accessJwtToken.Token, cookieOptions);
                return accessJwtToken;
            }
            catch (Exception ex)
            {
                switch (ex.Message)
                {
                    case "User id in jwt is invalid":
                        return BadRequest("User id in jwt is invalid");
                    case "Invalid refresh token.":
                        return Unauthorized("Invalid refresh token.");
                    case "Token expired.":
                        return Unauthorized("Token expired.");
                    default:
                        return BadRequest(ex.Message);
                }
            }
        }

        [HttpPost("addMockUsers")]
        public async Task<IActionResult> PostMockUsers([FromQuery] int numOfUsers = 10)
        {
            var users = await _authRepository.AddMockUsersAsync(numOfUsers);
            return Ok(users);
        }
    }
}