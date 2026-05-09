using Travel_Explorer.Application.Features.Reviews.Commands.CreateReview;
using Travel_Explorer.Application.Features.Reviews.Commands.DeleteReview;
using Travel_Explorer.Application.Features.Reviews.Commands.UpdateReview;
using Travel_Explorer.Application.Features.Reviews.Queries.GetReviewById;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages user reviews on destinations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ReviewsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReviewsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Returns a single review by its ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetReviewByIdQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Submits a new review for a destination. Requires the Traveler role.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
        {
            // TODO: Replace with actual authenticated user ID from JWT claims
            command.UserId = 1; // Placeholder for now

            var result = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates the current user's own review. Requires the Traveler role.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Traveler")]
        [ProducesResponseType(typeof(ReviewDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReviewCommand command)
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
        /// Soft-deletes a review.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteReviewCommand(id));

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
