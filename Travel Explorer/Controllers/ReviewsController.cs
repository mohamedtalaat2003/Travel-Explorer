using System.Security.Claims;
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
    [Route("api/Reviews")]
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
            var userId = GetCurrentUserId();
            if (userId.HasValue)
                command.UserId = userId.Value;

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
            var userId = GetCurrentUserId();

            if (userId.HasValue) command.UserId = userId.Value; 

            command.Id = id;

            var result = await _mediator.Send(command);
            return Ok(result);

        }

        /// <summary>
        /// Soft-deletes a review. Requires the Admin role.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteReviewCommand(id));
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
