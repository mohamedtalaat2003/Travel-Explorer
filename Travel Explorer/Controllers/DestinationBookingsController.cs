using Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.DeleteBooking;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingNotes;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingStatus;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.CancelBooking;
using Travel_Explorer.Application.Features.DestinationBookings.Queries.GetAllBookings;
using Travel_Explorer.Application.Features.DestinationBookings.Queries.GetBookingById;
using Travel_Explorer.Application.Features.DestinationBookings.Queries.GetMyBookings;

namespace Travel_Explorer.Controllers
{
    
    
    
    [ApiController]
    [Route("api/DestinationBookings")]
    [Produces("application/json")]
    public class DestinationBookingsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        
        
        
        [HttpPost]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(DestinationBookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        
        
        
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(DestinationBookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetBookingByIdQuery(id));
            return Ok(result);
        }

        
        
        
        [HttpGet("my")]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(IEnumerable<DestinationBookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyBookings([FromQuery] string? status = null)
        {
            var result = await _mediator.Send(new GetMyBookingsQuery(status));
            return Ok(result);
        }

        
        
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(IEnumerable<DestinationBookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll([FromQuery] string? status = null)
        {
            var result = await _mediator.Send(new GetAllBookingsQuery(status));
            return Ok(result);
        }

        
        
        
        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(DestinationBookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        
        
        
        [HttpPatch("{id:int}")]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(DestinationBookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateNotes(int id, [FromBody] UpdateBookingNotesCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        
        
        
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteBookingCommand(id));
            return NoContent();
        }

        
        
        
        [HttpPatch("{id:int}/cancel")]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _mediator.Send(new CancelDestinationBookingCommand(id));
            return Ok(result);
        }
    }
}


