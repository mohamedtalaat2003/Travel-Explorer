using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Commands.CreateBlog
{
    public class CreateBlogCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CreateBlogCommand, CreateBlogDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<CreateBlogDto> Handle(CreateBlogCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var blog = _mapper.Map<Blog>(request);
            blog.AuthorId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();
            blog.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Blog>().AddAsync(blog);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var spec = new BlogSpecification(blog.Id);
            var loaded = await _unitOfWork.Repository<Blog>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<CreateBlogDto>(loaded);
        }
    }
}
