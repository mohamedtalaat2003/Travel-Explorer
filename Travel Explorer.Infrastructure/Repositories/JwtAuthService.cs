using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs;
using Travel_Explorer.Application.DTOs.Account;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Application.Services;
using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class JwtAuthService(ApplicationDbContext _context, IOptions<JwtSettings> jwtSettingOptions, IUnitOfWork _unitOfWork) : IJwtAuthService
    {
        private readonly JwtSettings jwtSettings = jwtSettingOptions.Value;

        public async Task<ApplicationUser> RegisterAsync(RegisterDto request)
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


                user.UserName = request.UserName;
                user.Email = request.Email;
                user.PasswordHash = hashedPassword;
                user.Role = "User";//by default

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                throw;
            }

            return user;
        }

        public async Task<string> LoginAsync(LoginDto request)
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
            return await CreateToken(user);
        }

        public async Task<string> AssignUserAsync(AssignRoleDto request)
        {
            var user = await _context.Users.FindAsync(request.userId);

            if (user is null) return null;

            user.Role = request.newRole;
            await _context.SaveChangesAsync();

            return await CreateToken(user);
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
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Token));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.AccessTokenExpirationMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var tokenHash = HashToken(request.RefreshToken);
            var spec = new SpecificationUserRefreshToken(tokenHash);
         var storedToken = await  _unitOfWork.Repository<UserRefreshToken>().GenericEntitiesWithSpec(spec);

            if (storedToken == null)
                return null;

            if (storedToken.IsUsed)     
            {
                await RevokeAllTokenForUserAsync(storedToken.UserId.Value);
                return null;
            }

            if(storedToken.IsRevoked || storedToken.ExpiryDate <= DateTime.UtcNow)
            {
                return null;
            }

            storedToken.IsUsed = true;
            await _context.SaveChangesAsync();

            return await CreateTokenResponse(storedToken.User);
        }

        public async Task<bool> LogoutAync(int userId,string refreshToken)
        {
            var tokenHash = HashToken(refreshToken);

            var spec = new SpecificationUserRefreshToken(tokenHash, userId);
            var storedToken = await _unitOfWork.Repository<UserRefreshToken>().GenericEntitiesWithSpec(spec);

            if (storedToken == null || storedToken.IsRevoked)
                return false;

            storedToken.IsRevoked = true;

            await _context.SaveChangesAsync();

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
        private async Task<string> GenerateAndSaveRefreshTokenAsync(ApplicationUser user)
        {
            var refreshToken = GenerateRefreshToken();
            var tokeHash = HashToken(refreshToken);

            var refreshTokenEntity = new UserRefreshToken
            {
                UserId = user.Id,
                TokenHash = tokeHash,
                ExpiryDate = DateTime.UtcNow.AddDays(jwtSettings.RefreshTokenExpirationDays),
               CreatedAt = DateTime.UtcNow
            };
            
           await _context.UserRefreshTokens.AddAsync(refreshTokenEntity);
           await _context.SaveChangesAsync();

            return refreshToken;
        }

        private async Task RevokeAllTokenForUserAsync(int userId)
        {
            var spec = new SpecificationUserRefreshToken(userId);
            var activeToken = await _unitOfWork.Repository<UserRefreshToken>().ListSpecAsync(spec);

            foreach (var token in activeToken)
            {
                token.IsRevoked = true;
            }
            await _context.SaveChangesAsync();
        }

        private static string HashToken(string token)
        {
            using var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(token);
            var hash = sha512.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
    }
