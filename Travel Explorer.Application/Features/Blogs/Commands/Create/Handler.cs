using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Domain.Entities;

namespace Travel_Explorer.Application.Features.Blogs.Commands.Add
{
    public class Handler : IRequestHandler<CreateBlogCommand, CreateBlogDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        public Handler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<CreateBlogDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            var blog = _mapper.Map<Blog>(request);
            blog.AuthorId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

            try
            {
                blog.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Blog>().AddAsync(blog);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch(Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                throw new Exception("Exception occure when try to create");
                // Rethrow the exception to be handled by the caller
            }
            var spec = new BlogSpecification(blog.Id);
            var loaded = await _unitOfWork.Repository<Travel_Explorer.Domain.Entities.Blog>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<CreateBlogDto>(loaded);
        }
    }
}
