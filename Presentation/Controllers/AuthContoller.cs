using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Props;

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

        private record struct RegisterResponse(string Message);
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterProp registerProp)
        {
            try
            {
                await _authRepository.RegisterUserAsync(registerProp.Login, registerProp.Email, registerProp.Password);
                return Ok(new RegisterResponse("Registered."));
            }
            catch (AuthException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        public record struct JwtTokensResponse(JwtToken AccessToken, JwtToken RefreshToken);
        [HttpPost("login")]
        public async Task<ActionResult<JwtTokensResponse>> Login(LoginProp prop)
        {
            try
            {
                var (accessJwtToken, refreshJwtToken) = await _authRepository.LoginAsync(prop.LoginOrEmail, prop.Password);
                return Ok(new JwtTokensResponse(accessJwtToken, refreshJwtToken));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<AuthContoller>
        [HttpPost("refreshToken")]
        [Authorize]
        public async Task<ActionResult<JwtTokensResponse>> RefreshToken(string refreshToken)
        {
            try
            {
                var (accessToken, newRefreshJwtToken) = await _authRepository.RefreshToken(refreshToken);
                return Ok(new JwtTokensResponse(accessToken, newRefreshJwtToken));
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
    }
}