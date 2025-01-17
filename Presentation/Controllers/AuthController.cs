using DataLayer.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using Models.Props;
using NuGet.Protocol;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;

        public AuthController(IAuthRepository authRepository, IUserRepository userRepository)
        {
            _authRepository = authRepository;
            _userRepository = userRepository;
        }

        record Resp(string message);

        [HttpGet("hello")]
        public IActionResult Hello()
        {
            var message = "hello world---";
            return Ok(new Resp(message));
        }

        [HttpPost("register")]
        public async Task<ActionResult<JwtTokensResponse>> Register(RegisterProp registerProp)
        {
            try
            {
                var tokens = await _authRepository.RegisterUserAsync(registerProp.Login, registerProp.Email,
                    registerProp.Password, registerProp.ClerkId);
                return Ok(tokens);
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
                var (accessJwtToken, refreshJwtToken) = await _authRepository.LoginAsync(prop.Email, prop.Password);
                return Ok(new JwtTokensResponse(accessJwtToken, refreshJwtToken));
            }
            catch (AuthException exception)
            {
                return BadRequest(exception.Message);
            }
        }
        
        [HttpPost("OAuthSignIn")]
        public async Task<ActionResult<JwtTokensResponse>> OAuthSignIn(OAuthSignInProp prop)
        {
            try
            {
                var (accessJwtToken, refreshJwtToken) = await _authRepository.OAuthSignInAsync(prop.SessionId);
                return Ok(new JwtTokensResponse(accessJwtToken, refreshJwtToken));
            }
            catch (AuthException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        // POST api/<AuthContoller>
        [HttpPost("refreshToken")]
        [Authorize]
        public async Task<ActionResult<JwtTokensResponse>> RefreshToken([FromBody] string refreshToken)
        {
            try
            {
                var (accessToken, newRefreshJwtToken) = await _authRepository.RefreshToken(refreshToken);
                return Ok(new JwtTokensResponse(accessToken, newRefreshJwtToken));
            }
            catch (AuthException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        public record IsTakenResponse(bool isCredentialTaken);

        [HttpPost("IsCredentialTaken")]
        public async Task<ActionResult<IsTakenResponse>> IsTaken([FromQuery] string? login, [FromQuery] string? email)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email))
                return Ok(new IsTakenResponse(false));
            try
            {
                var isTaken = await _authRepository.IsCredentialTaken(login, email);
                return Ok(new IsTakenResponse(isTaken));
            }
            catch (AuthException exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpDelete()]
        public async Task<ActionResult<bool>> DeleteUser()
        {
            try
            {
                return await _authRepository.DeleteUser();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}