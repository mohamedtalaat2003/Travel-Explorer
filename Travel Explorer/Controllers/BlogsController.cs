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
    
    
    
    [ApiController]
    [Route("api/Blogs")]
    [Produces("application/json")]
    public class BlogsController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        

        
        
        
        
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResult<BlogDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] BlogSpecParams p)
            => Ok(await _mediator.Send(new GetAllBlogsQuery(p)));

        
        
        
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _mediator.Send(new GetBlogByIdQuery(id)));

        

        
        
        
        [HttpPost]
        [Authorize(Roles = "Author")]
        [ProducesResponseType(typeof(BlogDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateBlogCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        
        
        
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Author")]
        [ProducesResponseType(typeof(BlogDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBlogCommand command)
        {
            command = command with { Id = id };
            return Ok(await _mediator.Send(command));
        }

        
        
        
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,Author")]
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
