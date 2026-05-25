using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.GetAllBlogs
{
    public class GetAllBlogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllBlogsQuery, PaginatedResult<BlogDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<BlogDto>> Handle(
            GetAllBlogsQuery request, CancellationToken cancellationToken)
        {
            var p = request.Params;

            // CountAsync respects the spec's Criteria but ignores paging (via ignorePaging=true)
            var dataSpec = new BlogSpecification(p);

            var totalCount = await _unitOfWork.Repository<Blog>().CountAsync(dataSpec);
            var blogs      = await _unitOfWork.Repository<Blog>().ListSpecAsync(dataSpec);

            var dtos = _mapper.Map<IReadOnlyList<BlogDto>>(blogs);
            return new PaginatedResult<BlogDto>(dtos, totalCount, p.PageNumber, p.PageSize);
        }
    }
}
