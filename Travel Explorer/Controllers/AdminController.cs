using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Application.Features.Users.Commands.ApproveUser;
using Travel_Explorer.Application.Features.Users.Commands.BlockUser;
using Travel_Explorer.Application.Features.Users.Commands.ChangeUserRole;
using Travel_Explorer.Application.Features.Users.Commands.RejectUser;
using Travel_Explorer.Application.Features.Users.Commands.SoftDeleteUser;
using Travel_Explorer.Application.Features.Users.Commands.UnblockUser;
using Travel_Explorer.Application.Features.Users.Queries.GetAdminStatistics;
using Travel_Explorer.Application.Features.Users.Queries.GetAllUsers;
using Travel_Explorer.Application.Features.Users.Queries.GetUserById;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Controller responsible for administrative operations and user management.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        /// <summary>
        /// Retrieves all users with pagination, filtering, and search functionality.
        /// </summary>
        /// <param name="parameters">Pagination and filtering parameters.</param>
        /// <returns>List of users.</returns>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAll([FromQuery] UserSpecParams parameters)
        {
            var (users, totalCount) = await _mediator.Send(new GetAllUsersQuery(parameters));

            var paginationMetadata = new
            {
                totalCount,
                parameters.PageNumber,
                parameters.PageSize
            };

            Response.Headers.Append(
                "X-Pagination",
                System.Text.Json.JsonSerializer.Serialize(paginationMetadata));

            return Ok(users);
        }

        /// <summary>
        /// Retrieves user details by user id.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>User details.</returns>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            return user is null
                ? NotFound(new { Message = "User not found." })
                : Ok(user);
        }

        /// <summary>
        /// Blocks a specific user account.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Success message.</returns>
        [HttpPut("{id}/block")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> BlockUser(int id)
        {
            var success = await _mediator.Send(new BlockUserCommand(id));

            if (!success)
                return NotFound(new { Message = "User not found." });

            return Ok(new { Message = "User blocked successfully." });
        }

        /// <summary>
        /// Unblocks a specific user account.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Success message.</returns>
        [HttpPut("{id}/unblock")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UnBlockUser(int id)
        {
            var success = await _mediator.Send(new UnblockUserCommand(id));

            if (!success)
                return NotFound(new { Message = "User not found." });

            return Ok(new { Message = "User unblocked successfully." });
        }

        /// <summary>
        /// Changes the role of a specific user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <param name="model">Role update request.</param>
        /// <returns>Success message.</returns>
        [HttpPut("{id}/role")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ChangeRole(string id, [FromBody] ChangeRoleDto model)
        {
            var success = await _mediator.Send(
                new ChangeUserRoleCommand(id, model.NewRole));

            if (!success)
                return NotFound(new { Message = "User not found." });

            return Ok(new
            {
                Message = $"User role changed to {model.NewRole} successfully."
            });
        }

        /// <summary>
        /// Approves a user account.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Success message.</returns>
        [HttpPut("{id}/approve")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ApproveUser(int id)
        {
            var success = await _mediator.Send(new ApproveUserCommand(id));

            if (!success)
                return NotFound(new { Message = "User not found." });

            return Ok(new
            {
                Message = "User account approved successfully."
            });
        }

        /// <summary>
        /// Rejects a user account.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Success message.</returns>
        [HttpPut("{id}/reject")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RejectUser(int id)
        {
            var success = await _mediator.Send(new RejectUserCommand(id));

            if (!success)
                return NotFound(new { Message = "User not found." });

            return Ok(new
            {
                Message = "User account rejected successfully."
            });
        }

        /// <summary>
        /// Performs a soft delete for a specific user.
        /// </summary>
        /// <param name="id">User identifier.</param>
        /// <returns>Success message.</returns>
        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> SoftDelete(int id)
        {
            var success = await _mediator.Send(new SoftDeleteUserCommand(id));

            if (!success)
            {
                return NotFound(new
                {
                    Message = "User not found or already deleted."
                });
            }

            return Ok(new
            {
                Message = "User deleted successfully (soft delete)."
            });

        }

        /// <summary>
        /// Retrieves admin dashboard statistics.
        /// </summary>
        /// <returns>System statistics.</returns>
        [HttpGet("statistics")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(AdminStatisticsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetStatistics()
        {
            var stats = await _mediator.Send(new GetAdminStatisticsQuery());

            return Ok(stats);
        }
    }
}