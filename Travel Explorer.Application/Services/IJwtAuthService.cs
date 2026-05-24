using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs;
using Travel_Explorer.Application.DTOs.Account;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Domain.Interfaces
{
    public interface IJwtAuthService
    {
        Task<ApplicationUser> RegisterAsync(RegisterDto request, CancellationToken cancellationToken = default);
        public Task<TokenResponseDto> LoginAsync(LoginDto request);
        public Task<bool> LogoutAync(int userId, string refreshToken, CancellationToken cancellationToken = default);
        public Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);

        public Task<TokenResponseDto> AssignUserAsync(AssignRoleDto request , CancellationToken cancellationToken = default);

        string GetGoogleAuthorizationUrl(); 
        Task<ApplicationUser?> RegisterGoogleUserWithCodeAsyc(string code , CancellationToken cancellationToken = default);
    }
}
