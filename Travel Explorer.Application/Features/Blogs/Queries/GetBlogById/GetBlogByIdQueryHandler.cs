using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.GetBlogById
{
    public class GetBlogByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetBlogByIdQuery, BlogDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<BlogDto> Handle(GetBlogByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var spec = new BlogSpecification(request.Id);
            var blog = await _unitOfWork.Repository<Blog>().GenericEntitiesWithSpec(spec);

            if (blog == null || blog.IsDeleted)
            {
                throw new NotFoundException(nameof(Blog), request.Id);
            }

            if (!blog.IsPublished) 
            {
                throw new NotFoundException(nameof(Blog), request.Id);
            }

            return _mapper.Map<BlogDto>(blog);
        }
    }
}
