using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.GetAllBlogs
{
    public class GetAllBlogsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetAllBlogsQuery, PaginatedResult<BlogDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<PaginatedResult<BlogDto>> Handle(
            GetAllBlogsQuery request, CancellationToken cancellationToken)
        {
            var p = request.Params;

            // Security check: Only Admins or the Author themselves can view drafts
            if (p.IncludeDrafts)
            {
                bool isAuthorized = _currentUserService.IsAdmin ||
                                   (_currentUserService.UserId.HasValue && p.AuthorId.HasValue && _currentUserService.UserId.Value == p.AuthorId.Value);

                if (!isAuthorized)
                {
                    p.IncludeDrafts = false;
                }
            }

            var dataSpec = new BlogSpecification(p);

            var totalCount = await _unitOfWork.Repository<Blog>().CountAsync(dataSpec);
            var blogs      = await _unitOfWork.Repository<Blog>().ListSpecAsync(dataSpec);

            var dtos = _mapper.Map<IReadOnlyList<BlogDto>>(blogs);
            return new PaginatedResult<BlogDto>(dtos, totalCount, p.PageNumber, p.PageSize);
        }
    }
}
