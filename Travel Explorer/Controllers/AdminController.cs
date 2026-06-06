using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Application.Features.Users.Commands.ApproveAuthorRequest;
using Travel_Explorer.Application.Features.Users.Commands.BlockUser;
using Travel_Explorer.Application.Features.Users.Commands.ChangeUserRole;
using Travel_Explorer.Application.Features.Users.Commands.RejectAuthorRequest;
using Travel_Explorer.Application.Features.Users.Commands.SoftDeleteUser;
using Travel_Explorer.Application.Features.Users.Commands.UnblockUser;
using Travel_Explorer.Application.Features.Users.Queries.GetAdminStatistics;
using Travel_Explorer.Application.Features.Users.Queries.GetAllUsers;
using Travel_Explorer.Application.Features.Users.Queries.GetPendingAuthorRequests;
using Travel_Explorer.Application.Features.Users.Queries.GetUserById;

namespace Travel_Explorer.Controllers
{
    
    
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        
        
        
        
        
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

        

        
        
        
        
        [HttpGet("pending-author-requests")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetPendingAuthorRequests()
        {
            var users = await _mediator.Send(new GetPendingAuthorRequestsQuery());
            return Ok(users);
        }

        
        
        
        
        
        [HttpPut("{id}/approve-author")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ApproveAuthorRequest(int id)
        {
            var success = await _mediator.Send(new ApproveAuthorRequestCommand(id));

            if (!success)
                return NotFound(new { Message = "User not found or no pending Author request." });

            return Ok(new
            {
                Message = "Author request approved successfully. User is now an Author."
            });
        }

        
        
        
        
        
        [HttpPut("{id}/reject-author")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RejectAuthorRequest(int id)
        {
            var success = await _mediator.Send(new RejectAuthorRequestCommand(id));

            if (!success)
                return NotFound(new { Message = "User not found or no pending Author request." });

            return Ok(new
            {
                Message = "Author request rejected successfully."
            });
        }
    }
}