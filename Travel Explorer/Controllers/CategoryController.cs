using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Explorer.Application.Features.Blogs.Commands.Add;
using Travel_Explorer.Application.Features.Blogs.Commands.Delete;
using Travel_Explorer.Application.Features.Blogs.Commands.Update;
using Travel_Explorer.Application.Features.Blogs.Queries.Get;
using Travel_Explorer.Application.Features.Blogs.Queries.GetAll;
using Travel_Explorer.Application.Features.Categories.Commands.Create;
using Travel_Explorer.Application.Features.Categories.Commands.Delete;
using Travel_Explorer.Application.Features.Categories.Commands.Update;
using Travel_Explorer.Application.Features.Categories.Queires.Get;
using Travel_Explorer.Application.Features.Categories.Queires.GetAll;

namespace Travel_Explorer.Controllers
{
    [Authorize(Roles ="Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateCategoryCommand command)
        {
            if (command == null) return BadRequest("Category data is required.");
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(int Id, [FromBody] UpdateCategoryCommand command)
        {
            if (Id != command.Id || Id <= 0) return BadRequest("Category ID mismatch.");
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            if (Id <= 0) return BadRequest("Invalid Category ID.");
            var result = await _mediator.Send(new DeleteCategoryCommand(Id));

            if (!result)
                return NotFound("Category not found");

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetById(int Id)
        {
            if (Id <= 0) return BadRequest("Invalid Category ID.");
            var result = await _mediator.Send(new GetCategoryByIdQuery(Id));

            if (result == null) return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllCategoriesQuery(pageNumber, pageSize));
            if (result.Items.Count <= 0) return NotFound("No categories found.");
            return Ok(result);
        }
    }
}
