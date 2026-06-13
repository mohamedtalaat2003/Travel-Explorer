using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Commands.UpdateBlog
{
    public class UpdateBlogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<UpdateBlogCommand, BlogDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<BlogDto> Handle(UpdateBlogCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var blog = await _unitOfWork.Repository<Blog>().GetAsync(request.Id);

            if (blog == null || blog.IsDeleted)
            {
                throw new NotFoundException(nameof(Blog), request.Id);
            }

            var currentUserId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
            if (blog.AuthorId != currentUserId)
            {
                throw new ForbiddenAccessException();
            }

            _mapper.Map(request, blog);
            blog.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Blog>().Update(blog);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var spec = new BlogSpecification(blog.Id, includeDrafts: true);
            var loadedBlog = await _unitOfWork.Repository<Blog>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<BlogDto>(loadedBlog);
        }
    }
}
