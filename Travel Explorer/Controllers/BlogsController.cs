using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Application.Features.Blogs.Commands.CreateBlog;
using Travel_Explorer.Application.Features.Blogs.Commands.DeleteBlog;
using Travel_Explorer.Application.Features.Blogs.Commands.UpdateBlog;
using Travel_Explorer.Application.Features.Blogs.Queries.GetBlogById;
using Travel_Explorer.Application.Features.Blogs.Queries.GetAllBlogs;

namespace Travel_Explorer.Controllers
{
    /// <summary>
    /// Manages blog posts and articles.
    /// </summary>
    [ApiController]
    [Route("api/Blogs")]
    [Produces("application/json")]
    public class BlogsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // ─── Reads (anonymous) ────────────────────────────────────────────────

        /// <summary>
        /// Returns a paginated list of published blogs.
        /// Filter by <paramref name="p"/>.AuthorId and/or <paramref name="p"/>.CategoryId.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResult<BlogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] BlogSpecParams p)
            => Ok(await _mediator.Send(new GetAllBlogsQuery(p)));

        /// <summary>
        /// Returns a single published blog post by its ID.
        /// </summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _mediator.Send(new GetBlogByIdQuery(id)));

        // ─── Writes (Author) ──────────────────────────────────────────────────

        /// <summary>
        /// Submits a new blog post. Requires the Author role.
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Author")]
        [ProducesResponseType(typeof(CreateBlogDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateBlogCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates the current user's own blog post. Requires the Author role.
        /// </summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Author")]
        [ProducesResponseType(typeof(UpdateBlogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBlogCommand command)
        {
            command = command with { Id = id };
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Soft-deletes any blog post. Requires the Admin role.
        /// </summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteBlogCommand(id));
            return NoContent();
        }
    }
}
