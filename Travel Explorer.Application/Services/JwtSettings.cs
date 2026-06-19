using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Services
{
    public class JwtSettings
    {
        public string Token { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; } = 15;
        public int RefreshTokenExpirationDays { get; set; } = 7;

        public string SignInScheme { get; set; }
        public string GoogleClientId { get; set; }
        public string GoogleClientSecret { get; set; }
        
        private string _googleFrontendRedirectURl = string.Empty;
        public string GoogleFrontendRedirectURl 
        { 
            get => !string.IsNullOrEmpty(_googleFrontendRedirectURl) ? _googleFrontendRedirectURl : GoogleFrontendRedirectUri; 
            set => _googleFrontendRedirectURl = value; 
        }
        public string GoogleFrontendRedirectUri { get; set; } = string.Empty;
        public string GoogleFrontendloginRedirectUrl { get; set; }
    }
}
