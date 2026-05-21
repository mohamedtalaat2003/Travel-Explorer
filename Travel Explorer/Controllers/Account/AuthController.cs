using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Travel_Explorer.Application.DTOs;
using Travel_Explorer.Application.DTOs.Account;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController(IJwtAuthService _jwtAuthService, IMapper _mapper) : ControllerBase
    {
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
        public async Task<ActionResult> Logout([FromBody]RefreshTokenRequestDto request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userIdClaim) || int.TryParse(userIdClaim, out int userId))
                return Unauthorized("User is not authenticated.");

            var result = await _jwtAuthService.LogoutAync(userId, request.RefreshToken);
            if(!result)
                return BadRequest("Failed to logout. Please try again.");

            return Ok("Logged out successfully.");
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("assign-role")]
        public async Task<ActionResult> AssignRole(AssignRoleDto request)
        {
            var result = await _jwtAuthService.AssignUserAsync(request);

            if (result == null)
                return BadRequest("User not found or invalid role.");

            return Ok(result);
        }

    }
}
