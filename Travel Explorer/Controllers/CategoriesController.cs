using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.Features.Categories.Commands.CreateCategory;
using Travel_Explorer.Application.Features.Categories.Commands.DeleteCategory;
using Travel_Explorer.Application.Features.Categories.Commands.UpdateCategory;
using Travel_Explorer.Application.Features.Categories.Queries.GetCategoryById;
using Travel_Explorer.Application.Features.Categories.Queries.GetAllCategories;

namespace Travel_Explorer.Controllers
{
    
    
    
    [ApiController]
    [Route("api/Categories")]
    [Produces("application/json")]
    public class CategoriesController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        

        
        
        
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PaginatedResult<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] CategorySpecParams p)
            => Ok(await _mediator.Send(new GetAllCategoriesQuery(p)));

        
        
        
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
            => Ok(await _mediator.Send(new GetCategoryByIdQuery(id)));

        

        
        
        
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        
        
        
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCategoryCommand command)
        {
            command = command with { Id = id };
            return Ok(await _mediator.Send(command));
        }

        
        
        
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteCategoryCommand(id));
            return NoContent();
        }
    }
}
