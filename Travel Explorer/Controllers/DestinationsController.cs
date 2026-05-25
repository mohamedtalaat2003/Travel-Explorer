using Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination;
using Travel_Explorer.Application.Features.Destinations.Commands.DeleteDestination;
using Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination;
using Travel_Explorer.Application.Features.Destinations.Queries.GetAllDestinations;
using Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationActivities;
using Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationById;
using Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationReviews;
using Travel_Explorer.Application.Features.Destinations.Queries.GetTopRatedDestinations;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages travel destination resources.
    /// </summary>
    [ApiController]
    [Route("api/Destinations")]
    [Produces("application/json")]
    public class DestinationsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // ─── Reads (anonymous) ────────────────────────────────────────────────

        /// <summary>
        /// Returns a paginated list of active destinations.
        /// Supports optional filtering: <paramref name="p"/>.Keyword, .Location,
        /// .MinPrice, .MaxPrice, .CategoryId.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResult<DestinationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] DestinationSpecParams p)
            => Ok(await _mediator.Send(new GetAllDestinationsQuery(p)));

        /// <summary>
        /// Returns a single destination by its unique ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DestinationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _mediator.Send(new GetDestinationByIdQuery(id)));

        /// <summary>
        /// Returns the top-rated destinations ordered by AverageRating descending.
        /// </summary>
        [HttpGet("top-rated")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<DestinationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopRated([FromQuery] int count = 6)
            => Ok(await _mediator.Send(new GetTopRatedDestinationsQuery(count)));

        /// <summary>
        /// Returns all activities available at a specific destination.
        /// </summary>
        [HttpGet("{id:int}/activities")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ActivityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivities(int id)
            => Ok(await _mediator.Send(new GetDestinationActivitiesQuery(id)));

        /// <summary>
        /// Returns all user reviews for a specific destination.
        /// </summary>
        [HttpGet("{id:int}/reviews")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviews(int id)
            => Ok(await _mediator.Send(new GetDestinationReviewsQuery(id)));

        // ─── Writes (Admin only) ──────────────────────────────────────────────

        /// <summary>
        /// Creates a new destination. Requires the Admin role.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(DestinationDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateDestinationCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an existing destination. Requires the Admin role.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(DestinationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDestinationCommand command)
        {
            command.Id = id;
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Soft-deletes a destination. Requires the Admin role.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteDestinationCommand(id));
            return NoContent();
        }
    }
}
