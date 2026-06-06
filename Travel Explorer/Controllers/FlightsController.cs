using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Flights.Schedules;

using Travel_Explorer.Application.Features.Flights.Queries.GetAllFlightSchedules;
using Travel_Explorer.Application.Features.Flights.Queries.GetFlightScheduleById;
using Travel_Explorer.Application.Features.Flights.Commands.CreateFlightSchedule;
using Travel_Explorer.Application.Features.Flights.Commands.UpdateFlightSchedule;
using Travel_Explorer.Application.Features.Flights.Commands.DeleteFlightSchedule;

namespace Travel_Explorer.Controllers
{
    
    
    
    [ApiController]
    [Route("api/Flights")]
    [Produces("application/json")]
    public class FlightsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        
        
        
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<FlightScheduleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] FlightScheduleSpecParams p)
        {
            var result = await _mediator.Send(new GetAllFlightSchedulesQuery(p));
            return Ok(result);
        }

        
        
        
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(FlightScheduleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetFlightScheduleByIdQuery(id));
            return Ok(result);
        }

        
        
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(FlightScheduleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateFlightScheduleCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        
        
        
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(FlightScheduleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFlightScheduleCommand command)
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
            await _mediator.Send(new DeleteFlightScheduleCommand(id));
            return NoContent();
        }
    }
}
