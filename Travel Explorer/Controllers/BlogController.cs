using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Application.Features.Blogs.Commands.Add;
using Travel_Explorer.Application.Features.Blogs.Commands.Delete;
using Travel_Explorer.Application.Features.Blogs.Commands.Update;
using Travel_Explorer.Application.Features.Blogs.Queries.Get;
using Travel_Explorer.Application.Features.Blogs.Queries.GetAll;
using Travel_Explorer.Application.Features.Blogs.Queries.Search;
using Travel_Explorer.Application.Features.DestinationBookings.Commands.CreateBooking;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Controllers
{
    [Authorize(Roles = "Author")]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(IMapper mapper, IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateBlogCommand command)
        {
            if (command == null) return BadRequest("Blog data is required.");
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(int Id, [FromBody] UpdateBlogCommand command)
        {
            if (Id != command.Id || Id <= 0) return BadRequest("Blog ID mismatch.");
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(int Id)
        {
            if (Id <= 0) return BadRequest("Invalid Blog ID.");
            var result = await _mediator.Send(new DeleteBlogCommand(Id));

            if (!result)
                return NotFound("Blog not found");

            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<ActionResult> GetById(int Id)
        {
            if (Id <= 0) return BadRequest("Invalid Blog ID.");
            var result = await _mediator.Send(new GetBlogQuery(Id));

            if (result == null) return NotFound();

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllBlogsQuery(pageNumber, pageSize));
            if (result.Items.Count <= 0) return NotFound("No blogs found.");
            return Ok(result);
        }

        [HttpGet("search")]
        public ActionResult Search([FromQuery]int? AuthorId ,[FromQuery] int? CategoryId)
        {
            var author = _unitOfWork.Repository<ApplicationUser>().GetAsync(AuthorId);

            if (author == null)
                return NotFound("There is no author with this id");

            var category = _unitOfWork.Repository<Category>().GetAsync(CategoryId);

            if (category == null)
                return NotFound("Category is not found");

            var result = _mediator.Send(new SearchBlogsQuery(AuthorId, CategoryId)).Result;

            return Ok(result);
        }
      
    }
}
