using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.ContactMessage;

namespace Travel_Explorer.Application.Features.ContactMessages.GetAllContactMessages
{
    public record GetAllContactMessagesQuery(int PageNumber = 1, int PageSize = 10, bool? IsRead = null, int? UserId = null) 
        : IRequest<PaginatedResult<ContactMessageDto>>;
}
