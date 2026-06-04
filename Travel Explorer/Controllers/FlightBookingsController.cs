using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Flights.Bookings;

using Travel_Explorer.Application.Features.FlightBookings.Queries.GetAllFlightBookings;
using Travel_Explorer.Application.Features.FlightBookings.Queries.GetMyFlightBookings;
using Travel_Explorer.Application.Features.FlightBookings.Queries.GetFlightBookingById;
using Travel_Explorer.Application.Features.FlightBookings.Commands.CreateFlightBooking;
using Travel_Explorer.Application.Features.FlightBookings.Commands.UpdateFlightBookingStatus;
using Travel_Explorer.Application.Features.FlightBookings.Commands.CancelFlightBooking;

namespace Travel_Explorer.Controllers
{
    
    
    
    [ApiController]
    [Route("api/FlightBookings")]
    [Produces("application/json")]
    public class FlightBookingsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        
        
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaginatedResult<FlightBookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll([FromQuery] FlightBookingSpecParams p)
        {
            var result = await _mediator.Send(new GetAllFlightBookingsQuery(p));
            return Ok(result);
        }

        
        
        
        [HttpGet("my")]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(PaginatedResult<FlightBookingDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetMyBookings([FromQuery] FlightBookingSpecParams p)
        {
            var result = await _mediator.Send(new GetMyFlightBookingsQuery(p));
            return Ok(result);
        }

        
        
        
        [HttpGet("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(FlightBookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetFlightBookingByIdQuery(id));
            return Ok(result);
        }

        
        
        
        [HttpPost]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(FlightBookingDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateFlightBookingCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        
        
        
        [HttpPatch("{id:int}/status")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(FlightBookingDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] Travel_Explorer.Domain.Enums.BookingStatus status)
        {
            var result = await _mediator.Send(new UpdateFlightBookingStatusCommand(id, status));
            return Ok(result);
        }

        
        
        
        [HttpPatch("{id:int}/cancel")]
        [Authorize]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _mediator.Send(new CancelFlightBookingCommand(id));
            return Ok(result);
        }
    }
}
