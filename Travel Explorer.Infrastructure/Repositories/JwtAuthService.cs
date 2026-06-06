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
using Travel_Explorer.Domain.Enums;
using Travel_Explorer.Infrastructure.Data;

namespace Travel_Explorer.Infrastructure.Repositories
{
    public class JwtAuthService(ApplicationDbContext _context, IOptions<JwtSettings> jwtSettingOptions, IUnitOfWork _unitOfWork) : IJwtAuthService
    {
        private readonly JwtSettings _jwtSettings = jwtSettingOptions.Value;

        public async Task<ApplicationUser> RegisterAsync(RegisterDto request, CancellationToken cancellationToken = default)
        {
            
            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken)
                || await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
            {
                return null;
            }

            var user = new ApplicationUser();

            var hashedPassword = new PasswordHasher<ApplicationUser>()
                .HashPassword(user, request.Password);

            user.FullName = request.FullName;
            user.UserName = request.UserName;
            user.NormalizedUserName = request.UserName.ToUpperInvariant();
            user.Email = request.Email;
            user.NormalizedEmail = request.Email.ToUpperInvariant();
            user.PasswordHash = hashedPassword;
            user.Role = "Traveler"; 
            user.CreatedAt = DateTime.UtcNow;

            
            
            user.Status = AccountStatus.Approved;
            user.requestToBeAuthor = request.IWantToBeAuthor
                ? RequestToBeAuthor.Pending
                : RequestToBeAuthor.Rejected;

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return user;
        }

        public async Task<TokenResponseDto> LoginAsync(LoginDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName || u.Email == request.UserName);
            if (user == null)
            {
                return null;
            }

            if (user.IsBlocked || user.Status == AccountStatus.Pending || user.Status == AccountStatus.Rejected)
            {
                return null;
            }

            var passwordVerificationResult = new PasswordHasher<ApplicationUser>()
                .VerifyHashedPassword(user, user.PasswordHash, request.Password);

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            
            return await CreateTokenResponse(user);
        }

        public async Task<TokenResponseDto> AssignUserAsync(AssignRoleDto request,bool iWantToBeAuthor = false ,  CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FindAsync(request.userId);

            if (user is null) return null;

            if (iWantToBeAuthor)
            {
                
                user.requestToBeAuthor = RequestToBeAuthor.Approved;
                user.Role = "Author";
            }
            else
            {
                
                user.requestToBeAuthor = RequestToBeAuthor.Rejected;
                user.Role = request.newRole;
            }

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
                if (storedToken.UserId.HasValue)
                    await RevokeAllTokenForUserAsync(storedToken.UserId.Value);
                return null;
            }

            if (storedToken.IsRevoked || storedToken.ExpiryDate <= DateTime.UtcNow)
            {
                return null;
            }

            
            if (storedToken.User is null)
                return null;

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

        public async Task<ApplicationUser?> RegisterGoogleUserAsync(string email, string name, string googleId, CancellationToken cancellationToken = default)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId || u.Email == email, cancellationToken);

            if (existingUser != null)
            {
                if (string.IsNullOrEmpty(existingUser.GoogleId))
                {
                    existingUser.GoogleId = googleId;
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
                return existingUser;
            }

            var newUser = new ApplicationUser
            {
                UserName = name,
                NormalizedUserName = name.ToUpperInvariant(),
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                GoogleId = googleId,
                Role = "Traveler",
                PasswordHash = null,
                Status = AccountStatus.Approved,
                CreatedAt = DateTime.UtcNow,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            _context.Users.Add(newUser);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return newUser;
        }

        public async Task<TokenResponseDto?> LoginGoogleUserAsync(string googleId, string? email = null, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.GoogleId == googleId, cancellationToken);

            if (user == null && !string.IsNullOrEmpty(email))
            {
                user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
                if (user != null)
                {
                    user.GoogleId = googleId;
                    await _unitOfWork.SaveChangesAsync(cancellationToken);
                }
            }

            if (user == null)
                return null;

            string token = await CreateToken(user);
            var refreshToken = await GenerateAndSaveRefreshTokenAsync(user, cancellationToken);

            return new TokenResponseDto
            { AccessToken = token, RefreshToken = refreshToken };
        }
       
    }
}

