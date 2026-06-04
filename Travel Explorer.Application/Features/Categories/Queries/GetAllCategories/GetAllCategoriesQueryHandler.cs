using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllCategoriesQuery, PaginatedResult<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<CategoryDto>> Handle(
            GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var p = request.Params;

            
            _ = new CategorySpecification(new Common.Parameters.CategorySpecParams
            {
                PageNumber = p.PageNumber,
                PageSize = int.MaxValue   
            });

            
            var dataSpec = new CategorySpecification(p);

            var totalCount = await _unitOfWork.Repository<Category>().CountAsync(dataSpec);
            var categories  = await _unitOfWork.Repository<Category>().ListSpecAsync(dataSpec);

            var dtos = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);
            return new PaginatedResult<CategoryDto>(dtos, totalCount, p.PageNumber, p.PageSize);
        }
    }
}
