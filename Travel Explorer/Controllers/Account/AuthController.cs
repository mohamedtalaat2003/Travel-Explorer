using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Travel_Explorer.Application.DTOs;
using Travel_Explorer.Application.DTOs.Account;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Application.Services;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IJwtAuthService _jwtAuthService, IOptions<JwtSettings> jwtSettingOptions) : ControllerBase
    {
        private readonly JwtSettings _jwtSettings = jwtSettingOptions.Value;
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterDto request)
        {
            var user = await _jwtAuthService.RegisterAsync(request);
            if (user == null)
                return BadRequest("User already exists.");

            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FullName,
                user.Role
            });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginDto request)
        {
            var token = await _jwtAuthService.LoginAsync(request);

            if (token == null)
                return Unauthorized("Invalid username or password.");

            return Ok(new { Token = token });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto refreshToken)
        {
            var result = await _jwtAuthService.RefreshTokenAsync(refreshToken);

            if (result is null || string.IsNullOrEmpty(result.AccessToken) || string.IsNullOrEmpty(result.RefreshToken))
                return Unauthorized("Invalid or expired refresh token");

            return Ok(result);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<ActionResult> Logout([FromBody] RefreshTokenRequestDto request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("User is not authenticated.");

            var result = await _jwtAuthService.LogoutAync(userId, request.RefreshToken);
            if (!result)
                return BadRequest("Failed to logout. Please try again.");

            return Ok("Logged out successfully.");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<ActionResult> AssignRole(AssignRoleDto request , bool IWantToBeAuthor = false)
        {
            var result = await _jwtAuthService.AssignUserAsync(request , IWantToBeAuthor);

            if (result == null)
                return BadRequest("User not found or invalid role.");

            return Ok(result);
        }

        [HttpGet("google-register")]
        [AllowAnonymous]
        public ActionResult InitiateGoogleRegister()
        {
            
            var redirectUrl = Url.Action("GoogleRegisterCallback", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
           return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-register-callback")]
        [AllowAnonymous]
        public async Task<ActionResult> GoogleRegisterCallback([FromQuery] string? code = null, [FromQuery] string? error = null)
        {
            var result = await HttpContext.AuthenticateAsync("ExternalCookie");
            if(!result.Succeeded|| result.Principal == null)
            {
                return Redirect($"{_jwtSettings.GoogleFrontendRedirectURl}?error=AuthenticationFailed");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            await HttpContext.SignOutAsync("ExternalCookie");

            if(string.IsNullOrEmpty(email) ||string.IsNullOrEmpty(googleId))
            {
                return Redirect($"{_jwtSettings.GoogleFrontendRedirectURl}?error=MissingEmailOrGoogleId");
            }

            var userName = name ?? email.Split('@')[0];

            var user = await _jwtAuthService.RegisterGoogleUserAsync(email, userName, googleId);

            var encodeUserName = Uri.EscapeDataString(userName);
            var encodeEmail = Uri.EscapeDataString(email);

            return Redirect($"{_jwtSettings.GoogleFrontendRedirectURl}?success=success&username={encodeUserName}&email={encodeEmail}");
        }

        [HttpGet("google-login")]
        [AllowAnonymous]
        public ActionResult IntiateGoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleLoginCallback", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties,GoogleDefaults.AuthenticationScheme);
        }


        [HttpGet("google-login-callback")]
        [AllowAnonymous]
        public async Task<ActionResult> GoogleLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync("ExternalCookie");
            if (!result.Succeeded || result.Principal == null)
            {
                return Redirect($"{_jwtSettings.GoogleFrontendloginRedirectUrl}?error=AuthenticationFailed");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            await HttpContext.SignOutAsync("ExternalCookie");

            if(string.IsNullOrEmpty(googleId))
            {
                return Redirect($"{_jwtSettings.GoogleFrontendloginRedirectUrl}?error=MissingGoogleId");
            }
            
            var token = await _jwtAuthService.LoginGoogleUserAsync(googleId, email);
            if(token == null)
                return Redirect($"{_jwtSettings.GoogleFrontendRedirectURl}?error=UserNotRegistered");

            var accessToken = Uri.EscapeDataString(token.AccessToken);
            var refreshToken = Uri.UnescapeDataString(token.RefreshToken);

            return Redirect($"{_jwtSettings.GoogleFrontendloginRedirectUrl}?success=success&accessToken={accessToken}&refreshToken={refreshToken}");

        }
    }
}
