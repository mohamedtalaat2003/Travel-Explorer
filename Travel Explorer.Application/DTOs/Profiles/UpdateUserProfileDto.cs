
namespace Travel_Explorer.Application.DTOs.Profiles
{
    public class UpdateUserProfileDto
    {
        [StringLength(1000)]
        public string? Bio { get; set; }

        [StringLength(500)]
        public string? AvatarUrl { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        public DateTime? DateOfBirth { get; set; }

        [StringLength(50)]
        public string? PassportNumber { get; set; }
    }
}
