using System.ComponentModel.DataAnnotations;

namespace Travel_Explorer.Application.DTOs.Contact
{
    public class CreateContactMessageDto
    {
        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; }

        [Required]
        [StringLength(300)]
        public string Subject { get; set; }

        [Required]
        [StringLength(2000)]
        public string Message { get; set; }
    }
}
