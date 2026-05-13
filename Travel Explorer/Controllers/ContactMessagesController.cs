using System.Security.Claims;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.ContactMessage;
using Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage;
using Travel_Explorer.Application.Features.ContactMessages.GetAllContactMessages;
using Travel_Explorer.Application.Features.ContactMessages.GetContactMessageById;
using Travel_Explorer.Application.Features.ContactMessages.DeleteContactMessage;
using Travel_Explorer.Application.Features.ContactMessages.MarkAsRead;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages contact messages submitted via the Contact Us form.
    /// </summary>
    [ApiController]
    [Route("api/ContactMessages")]
    [Produces("application/json")]
    public class ContactMessagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ContactMessagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a paginated list of all contact messages. Supports filtering by read status.
        /// Requires Admin role.
        /// </summary>
        
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(PaginatedResult<ContactMessageDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            [FromQuery] int pageNumber = 1, 
            [FromQuery] int pageSize = 10, 
            [FromQuery] bool? isRead = null)
        {
            var currentUserId = GetCurrentUserId();

            bool isAdmin = User.IsInRole("Admin");

            var result = await _mediator.Send(new GetAllContactMessagesQuery(pageNumber, pageSize, isRead,isAdmin? null :currentUserId));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific contact message by ID. Pure read — no side effects. Requires Admin role.
        /// </summary>
        
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ContactMessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessage(int id)
        {
            
            var currentUserId = GetCurrentUserId();

            bool isAdmin = User.IsInRole("Admin");

            var result = await _mediator.Send(new GetContactMessageByIdQuery(id, isAdmin ? null : currentUserId));
            return Ok(result);

        }

        /// <summary>
        /// Submits a new contact message. Accessible by anyone.
        /// If the user is authenticated, their UserId is automatically attached.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ContactMessageDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateContactMessageCommand command)
        {
            // Attach UserId if the sender is authenticated
            var userId = GetCurrentUserId();
            if (userId.HasValue)
                command = command with { UserId = userId.Value };

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMessage), new { id = result.Id }, result);
        }

        /// <summary>
        /// Marks a contact message as read. Requires Admin role.
        /// </summary>
        
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _mediator.Send(new MarkAsReadCommand(id));
            return Ok(result) ;

        }

        /// <summary>
        /// Deletes a contact message by ID. Requires Admin role.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteContactMessageCommand(id));
            return NoContent();

        }

        /// <summary>
        /// Extracts the current user's ID from the JWT claims.
        /// </summary>
        private int? GetCurrentUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return null;
            return userId;
        }
    }
}
