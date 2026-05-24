using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Travel_Explorer.Application.DTOs;
using Travel_Explorer.Application.DTOs.Account;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Application.Services;
using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class JwtAuthService(ApplicationDbContext _context, IOptions<JwtSettings> jwtSettingOptions, IUnitOfWork _unitOfWork) : IJwtAuthService
    {
        private readonly JwtSettings _jwtSettings = jwtSettingOptions.Value;

        public async Task<ApplicationUser> RegisterAsync(RegisterDto request, CancellationToken cancellationToken = default)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName))
            {
                return null;
            }

            var user = new ApplicationUser();

            try
            {
                var hashedPassword = new PasswordHasher<ApplicationUser>()
                    .HashPassword(user, request.Password);

                user.FullName = request.FullName;
                user.UserName = request.UserName;
                user.Email = request.Email;
                user.PasswordHash = hashedPassword;
                user.Role = "Traveler";//by default
                user.CreatedAt = DateTime.UtcNow;

                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                throw;
            }

            return user;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            if (user == null)
            {
                return null;
            }

            var passwordVerificationResult = new PasswordHasher<ApplicationUser>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            //var token = JwtTokenGenerator.GenerateToken(user, jwtSettings);
            return await CreateTokenResponse(user);
        }

        public async Task<TokenResponseDto> AssignUserAsync(AssignRoleDto request, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(request.userId);

            if (user is null) return null;

            user.Role = request.newRole;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return await CreateTokenResponse(user);
        }

        private async Task<string> CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.Name , user.UserName),
                new Claim (ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim (ClaimTypes.Role, user.Role)
            };
            //var token = JwtTokenGenerator.GenerateToken(user, jwtSettings);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Token));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
        {
            var tokenHash = HashToken(request.RefreshToken);
            var spec = new SpecificationUserRefreshToken(tokenHash);
            var storedToken = await _unitOfWork.Repository<UserRefreshToken>().GenericEntitiesWithSpec(spec);

            if (storedToken == null)
                return null;

            if (storedToken.IsUsed)
            {
                await RevokeAllTokenForUserAsync(storedToken.UserId.Value);
                return null;
            }

            if (storedToken.IsRevoked || storedToken.ExpiryDate <= DateTime.UtcNow)
            {
                return null;
            }

            storedToken.IsUsed = true;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return await CreateTokenResponse(storedToken.User);
        }

        public async Task<bool> LogoutAync(int userId, string refreshToken, CancellationToken cancellationToken = default)
        {
            var tokenHash = HashToken(refreshToken);

            var spec = new SpecificationUserRefreshToken(tokenHash, userId);
            var storedToken = await _unitOfWork.Repository<UserRefreshToken>().GenericEntitiesWithSpec(spec);

            if (storedToken == null || storedToken.IsRevoked)
                return false;

            storedToken.IsRevoked = true;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }

        private async Task<TokenResponseDto> CreateTokenResponse(ApplicationUser user)
        {

            return new TokenResponseDto
            {
                AccessToken = await CreateToken(user),
                RefreshToken = await GenerateAndSaveRefreshTokenAsync(user)
            };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private async Task<string> GenerateAndSaveRefreshTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var refreshToken = GenerateRefreshToken();
            var tokeHash = HashToken(refreshToken);

            var refreshTokenEntity = new UserRefreshToken
            {
                UserId = user.Id,
                TokenHash = tokeHash,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow
            };

            await _context.UserRefreshTokens.AddAsync(refreshTokenEntity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return refreshToken;
        }

        private async Task RevokeAllTokenForUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            var spec = new SpecificationUserRefreshToken(userId);
            var activeToken = await _unitOfWork.Repository<UserRefreshToken>().ListSpecAsync(spec);

            foreach (var token in activeToken)
            {
                token.IsRevoked = true;
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        private static string HashToken(string token)
        {
            using var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = sha512.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public string GetGoogleAuthorizationUrl()
        {
            var clientId = _jwtSettings.GoogleClientId;
            var redirectUri = _jwtSettings.GoogleRedirectUrl;
            var scope = Uri.EscapeDataString("openid profile email");

            return $"https://accounts.google.com/o/oauth2/v2/auth?" +
                    $"client_id={clientId}&" +
                    $"redirect_uri={redirectUri}&" +
                    $"response_type=code&" +
                    $"scope={scope}&" +
                    $"state=register";
        }

        public async Task<ApplicationUser?> RegisterGoogleUserWithCodeAsyc(string code, CancellationToken cancellationToken = default)
        {
            try
            {
                using var httpClient = new HttpClient();

                var tokenRequestParameters = new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", _jwtSettings.GoogleClientId },
                { "client_secret", _jwtSettings.GoogleClientSecret },
                { "redirect_uri", _jwtSettings.GoogleRedirectUrl },
                { "grant_type", "authorization_code" }
            };

                var response = httpClient.PostAsync("https://oauth2.googleapis.com/token",
                    new FormUrlEncodedContent(tokenRequestParameters)).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var tokenResult = await response.Content.ReadFromJsonAsync<GoogleTokenResponse>();
                if (tokenResult == null || string.IsNullOrEmpty(tokenResult.IdToken))
                {
                    return null;
                }

                //Validate googleId Token
                var validationSettings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string> { _jwtSettings.GoogleClientId }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(tokenResult.IdToken, validationSettings);

                string email = payload.Email;
                string Name = payload.Name ?? payload.GivenName ?? email.Split('@')[0];
                string googleId = payload.Subject;

                //check if the user already exists

                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId || u.Email == email, cancellationToken);

                if (existingUser != null)
                {
                    //already Registered user
                    return existingUser;
                }

                var newUser = new ApplicationUser
                {
                    UserName = Name,
                    Email = email,
                    GoogleId = googleId,
                    Role = "User",
                    PasswordHash = null
                };

                _context.Users.Add(newUser);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return newUser;
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                return null;
            }

        }

        // Simple helper class for parsing token endpoint response
        private class GoogleTokenResponse
        {
            [System.Text.Json.Serialization.JsonPropertyName("id_token")]
            public string? IdToken { get; set; }
        }
    }
}
