using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.ContactMessage;
using Travel_Explorer.Application.Features.ContactMessages.CreateContactMessage;
using Travel_Explorer.Application.Features.ContactMessages.GetAllContactMessages;
using Travel_Explorer.Application.Features.ContactMessages.GetContactMessageById;
using Travel_Explorer.Application.Features.ContactMessages.DeleteContactMessage;
using Travel_Explorer.Application.Features.ContactMessages.MarkAsRead;

namespace Travel_Explorer.Controllers
{
    
    
    
    [ApiController]
    [Route("api/ContactMessages")]
    [Produces("application/json")]
    public class ContactMessagesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        
        
        
        

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaginatedResult<ContactMessageDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] ContactMessageSpecParams p)
        {
            var result = await _mediator.Send(new GetAllContactMessagesQuery(p));
            return Ok(result);
        }

        
        
        
        
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(ContactMessageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMessage(int id)
        {
            var result = await _mediator.Send(new GetContactMessageByIdQuery(id));
            return Ok(result);

        }

        
        
        
        
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ContactMessageDto), StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromBody] CreateContactMessageCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetMessage), new { id = result.Id }, result);
        }

        
        
        
        
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var result = await _mediator.Send(new MarkAsReadCommand(id));
            return Ok(result) ;

        }

        
        
        
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteContactMessageCommand(id));
            return NoContent();

        }
    }
}
