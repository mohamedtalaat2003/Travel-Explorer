using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Common.Parameters
{
    public class UserSpecParams : PaginationParams
    {
        public string? SearchTerm { get; set; }
        public Gender? Gender { get; set; }
        public bool? IsBlocked { get; set; }
        public AccountStatus? Status { get; set; }
    }
}
