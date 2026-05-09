using Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination;
using Travel_Explorer.Application.Features.Destinations.Commands.DeleteDestination;
using Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination;
using Travel_Explorer.Application.Features.Destinations.Queries.GetAllDestinations;
using Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationActivities;
using Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationById;
using Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationReviews;
using Travel_Explorer.Application.Features.Destinations.Queries.GetTopRatedDestinations;
using Travel_Explorer.Application.Features.Destinations.Queries.SearchDestinations;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages travel destination resources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DestinationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DestinationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns a paginated list of all active (non-deleted) destinations.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<DestinationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllDestinationsQuery(pageNumber, pageSize));
            return Ok(result);
        }

        /// <summary>
        /// Returns a single destination by its unique ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(DestinationDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetDestinationByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Searches and filters destinations by keyword, location, price range, or category.
        /// </summary>
        [HttpGet("search")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<DestinationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Search(
            [FromQuery] string? q,
            [FromQuery] string? location,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? categoryId)
        {
            var result = await _mediator.Send(new SearchDestinationsQuery(q, location, minPrice, maxPrice, categoryId));
            return Ok(result);
        }

        /// <summary>
        /// Returns the top-rated destinations ordered by AverageRating descending.
        /// </summary>
        [HttpGet("top-rated")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<DestinationDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTopRated([FromQuery] int count = 6)
        {
            var result = await _mediator.Send(new GetTopRatedDestinationsQuery(count));
            return Ok(result);
        }

        /// <summary>
        /// Returns all activities available at a specific destination.
        /// </summary>
        [HttpGet("{id:int}/activities")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ActivityDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivities(int id)
        {
            var result = await _mediator.Send(new GetDestinationActivitiesQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Returns all user reviews for a specific destination.
        /// </summary>
        [HttpGet("{id:int}/reviews")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(IEnumerable<ReviewDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReviews(int id)
        {
            var result = await _mediator.Send(new GetDestinationReviewsQuery(id));
            return Ok(result);
        }

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
            // Ensure ID from URL matches the command ID
            if (id != command.Id)
                return BadRequest("ID mismatch between URL and body.");

            var result = await _mediator.Send(command);

            if (result == null)
                return NotFound();

            return Ok(result);
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
            var result = await _mediator.Send(new DeleteDestinationCommand(id));

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
