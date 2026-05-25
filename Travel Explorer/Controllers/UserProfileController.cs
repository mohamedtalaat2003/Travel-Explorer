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
    public class UserProfileController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Retrieves the profile of the currently authenticated user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(UserProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProfile()
        {
            var result = await _mediator.Send(new GetUserProfileQuery());
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
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
