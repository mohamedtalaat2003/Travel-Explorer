using AutoMapper;
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
    [AllowAnonymous]
    public class AuthController(IJwtAuthService _jwtAuthService, IMapper _mapper, IOptions<JwtSettings> jwtSettingOptions) : ControllerBase
    {
        private readonly JwtSettings _jwtSettings = jwtSettingOptions.Value;
        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto request)
        {
            var userDto = await _jwtAuthService.RegisterAsync(request);
            if (userDto == null)
                return BadRequest("User already exists.");

            var user = _mapper.Map<ApplicationUser>(userDto);

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto request)
        {
            var token = await _jwtAuthService.LoginAsync(request);

            if (token == null)
                return Unauthorized("Invalid username or password.");

            return Ok(new { Token = token });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken(RefreshTokenRequestDto refreshToken)
        {
            var result = await _jwtAuthService.RefreshTokenAsync(refreshToken);

            if (result is null || string.IsNullOrEmpty(result.AccessToken) || string.IsNullOrEmpty(result.RefreshToken))
                return Unauthorized("Invalid or expired refresh token");

            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<ActionResult> Logout([FromBody] RefreshTokenRequestDto request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim) || int.TryParse(userIdClaim, out int userId))
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
        public ActionResult InitiateGoogleRegister()
        {
            //challange google schema and tell it to redirct to our GoogleRegisterCallback
            var redirectUrl = Url.Action("GoogleRegisterCallback", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
           return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-register-callback")]
        public async Task<ActionResult> GoogleRegisterCallback([FromQuery] string code, [FromQuery] string? error)
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

            await HttpContext.SignOutAsync("ExternamCookie");

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
        public ActionResult IntiateGoogleLogin()
        {
            var redirectUrl = Url.Action("GoogleLoginCallback", "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            return Challenge(properties,GoogleDefaults.AuthenticationScheme);
        }


        [HttpGet("google-login-callback")]
        public async Task<ActionResult> GoogleLoginCallback()
        {
            var result = await HttpContext.AuthenticateAsync("ExternalCookie");
            if (!result.Succeeded || result.Principal == null)
            {
                return Redirect($"{_jwtSettings.GoogleFrontendloginRedirectUrl}?error=AuthenticationFailed");
            }

            var claims = result.Principal.Identities.FirstOrDefault()?.Claims;
            var googleId = claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            await HttpContext.SignOutAsync("ExternalCookie");

            if(string.IsNullOrEmpty(googleId))
            {
                return Redirect($"{_jwtSettings.GoogleFrontendloginRedirectUrl}?error=MissingGoogleId");
            }
            
            var token = await _jwtAuthService.LoginGoogleUserAsync(googleId);
            if(token == null)
                return Redirect($"{_jwtSettings.GoogleFrontendRedirectURl}?error=UserNotRegistered");

            var accessToken = Uri.EscapeDataString(token.AccessToken);
            var refreshToken = Uri.UnescapeDataString(token.RefreshToken);

            return Redirect($"{_jwtSettings.GoogleFrontendloginRedirectUrl}?success=success&accessToken={accessToken}&refreshToken={refreshToken}");

        }
    }
}
