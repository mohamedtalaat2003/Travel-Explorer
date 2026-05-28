using Travel_Explorer.Application.DTOs.Profiles;
using Travel_Explorer.Application.Features.Profiles.Queries.GetUserProfile;
using Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile;
using Travel_Explorer.Application.Features.Users.Commands.RequestAuthorRole;
using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages user profile information.
    /// </summary>
    [ApiController]
    [Route("api/UserProfile")]
    [Authorize]
    [Produces("application/json")]
    public class UserProfileController(IMediator mediator, ICurrentUserService currentUserService) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ICurrentUserService _currentUserService = currentUserService;

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

        /// <summary>
        /// Submits a request for the current user to become an Author.
        /// The request goes to Admin for approval.
        /// </summary>
        [HttpPost("request-author")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RequestAuthorRole()
        {
            var userId = _currentUserService.UserId;
            if (!userId.HasValue)
                return Unauthorized(new { Message = "User is not authenticated." });

            var success = await _mediator.Send(new RequestAuthorRoleCommand(userId.Value));

            if (!success)
                return BadRequest(new { Message = "Unable to submit Author request. You may already have a pending request or are already an Author." });

            return Ok(new { Message = "Author request submitted successfully. Waiting for Admin approval." });
        }
    }
}
