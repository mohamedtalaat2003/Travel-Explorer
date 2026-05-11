using System.Text.Json.Serialization;
using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage
{
    public record CreateContactMessageCommand(
        [Required][StringLength(200, MinimumLength = 3)] string FullName,
        [Required][EmailAddress][StringLength(200)] string Email,
        [Required][StringLength(300)] string Subject,
        [Required][StringLength(2000)] string Message 
    ) : IRequest<ContactMessageDto>
    {
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
