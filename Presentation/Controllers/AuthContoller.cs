using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<ActionResult<User>> Register(RegisterProp registerProp)
        {
            var user = await _authRepository.RegisterUserAsync(registerProp.Login, registerProp.Email, registerProp.Password);
            return Ok();
        }

        public record struct LoginResponse(JwtToken accessToken, JwtToken refreshToken);
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginProp prop)
        {
            try
            {
                var (accessJwtToken, refreshJwtToken) = await _authRepository.LoginAsync(prop.LoginOrEmail, prop.Password);
                return Ok(new LoginResponse(accessJwtToken, refreshJwtToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<AuthContoller>
        [HttpPost("refreshToken")]
        [Authorize]
        public async Task<ActionResult<JwtToken>> RefreshToken()
        {
            var refreshToken = Request.Headers.Authorization.ToString().Substring("Bearer ".Length);

            try
            {
                if (refreshToken is null)
                {
                    throw new Exception("No refresh token in headers");
                }
                    
                var (_, newRefreshJwtToken) = await _authRepository.RefreshToken(refreshToken);
                return Ok(newRefreshJwtToken);
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