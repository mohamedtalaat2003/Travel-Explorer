using Travel_Explorer.Application.Features.Activities.Commands.CreateActivity;
using Travel_Explorer.Application.Features.Activities.Commands.DeleteActivity;
using Travel_Explorer.Application.Features.Activities.Commands.UpdateActivity;
using Travel_Explorer.Application.Features.Activities.Queries.GetActivityById;
using Travel_Explorer.Application.Features.Activities.Queries.GetAllActivities;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages activity resources.
    /// </summary>
    [ApiController]
    [Route("api/Activities")]
    [Produces("application/json")] // response json only
    public class ActivitiesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Returns all active activities across all destinations.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ActivityDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(int? destinationId)
        {
            var result = await _mediator.Send(new GetAllActivitiesQuery(destinationId));
            return Ok(result);
        }

        /// <summary>
        /// Returns a single activity by its ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ActivityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetActivityByIdQuery(id));
            return Ok(result);

        }

        /// <summary>
        /// Creates a new activity linked to a destination. Requires the Admin role.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ActivityDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateActivityCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing activity. Requires the Admin role.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ActivityDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateActivityCommand command)
        {
            // Set the ID from the URL directly into the command
            command.Id = id;

            var result = await _mediator.Send(command);
            return Ok(result);

        }

        /// <summary>
        /// Soft-deletes an activity. Requires the Admin role.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteActivityCommand(id));
            return NoContent();

        }
    }
}
