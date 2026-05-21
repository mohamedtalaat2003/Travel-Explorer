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
        Task<ApplicationUser> RegisterAsync(RegisterDto request);
        public Task<string> LoginAsync(LoginDto request);
        public Task<bool> LogoutAync(int userId, string refreshToken);
        public Task<TokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);


        public Task<string> AssignUserAsync(AssignRoleDto request);



    }
}
