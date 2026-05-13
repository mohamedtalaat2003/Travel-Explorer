using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Commands.Update
{
    public class Handler : IRequestHandler<UpdateBlogCommand, UpdateBlogDto>
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

        public async Task<UpdateBlogDto> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            var blog = await _unitOfWork.Repository<Blog>().GetAsync(request.Id);

            blog.AuthorId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

            if (blog == null)
                throw new KeyNotFoundException($"Blog with id {request.Id} not found.");

            _mapper.Map(request, blog);

            try
            {
                blog.UpdatedAt = DateTime.UtcNow;
                _unitOfWork.Repository<Blog>().Update(blog);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework like Serilog, NLog, etc.)
                throw new Exception("Exception occure when try to update");
                // Rethrow the exception to be handled by the caller
            }

            var spec = new BlogSpecification(blog.Id);
            var loaded = await _unitOfWork.Repository<Blog>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<UpdateBlogDto>(loaded);
        }
    }
}
