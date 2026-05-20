using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Travel_Explorer.Application.Common
{
    public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public int? UserId 
        { 
            get
            {
                var id = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
                return int.TryParse(id, out var userId) ? userId : null;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return _httpContextAccessor.HttpContext?.User.IsInRole("Admin") ?? false;
            }
        }
    }
}
