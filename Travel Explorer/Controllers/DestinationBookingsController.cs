using Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.DeleteBooking;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingNotes;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingStatus;
using Travel_Explorer.Application.Features.DestinationBookings.Queries.GetAllBookings;
using Travel_Explorer.Application.Features.DestinationBookings.Queries.GetBookingById;
using Travel_Explorer.Application.Features.DestinationBookings.Queries.GetMyBookings;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages destination booking operations.
    /// </summary>
    [ApiController]
    [Route("api/destination-bookings")]
    [Produces("application/json")]
    public class DestinationBookingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DestinationBookingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new destination booking. Requires the Traveler role.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(DestinationBookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
        {
            // TODO: Replace with actual authenticated user ID from JWT claims
            command.UserId = 1; // Placeholder for now

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Returns a single booking by its ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(DestinationBookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetBookingByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Returns all bookings belonging to the currently authenticated Traveler.
        /// </summary>
        [HttpGet("my")]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(IEnumerable<DestinationBookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyBookings([FromQuery] string? status = null)
        {
            // TODO: Replace with actual authenticated user ID from JWT claims
            var userId = 0;

            var result = await _mediator.Send(new GetMyBookingsQuery(userId, status));
            return Ok(result);
        }

        /// <summary>
        /// Returns all bookings in the system. Requires the Admin role.
        /// </summary>
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

        /// <summary>
        /// Updates the status of a specific booking.
        /// </summary>
        [HttpPatch("{id:int}/status")]
        [Authorize]
        [ProducesResponseType(typeof(DestinationBookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateBookingStatusCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Updates the notes on a booking. Accessible by the booking owner (Traveler) only.
        /// </summary>
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

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Soft-deletes a booking record. Requires the Admin role.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteBookingCommand(id));

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
