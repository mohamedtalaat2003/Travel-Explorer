using System.Security.Claims;
using Travel_Explorer.Application.DTOs.Profiles;
using Travel_Explorer.Application.Features.Profiles.Queries.GetUserProfile;
using Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages user profile information.
    /// </summary>
    [ApiController]
    [Route("api/UserProfile")]
    [Authorize]
    [Produces("application/json")]
    public class UserProfileController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserProfileController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves the profile of the currently authenticated user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Unauthorized();

            var result = await _mediator.Send(new GetUserProfileQuery(userId.Value));
            return Ok(result);

        }

        /// <summary>
        /// Updates the profile of the currently authenticated user.
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserProfileCommand command)
        {
            var userId = GetCurrentUserId();
            if (!userId.HasValue) return Unauthorized();

            command.UserId = userId.Value;
            var result = await _mediator.Send(command);
            return Ok(result);

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
