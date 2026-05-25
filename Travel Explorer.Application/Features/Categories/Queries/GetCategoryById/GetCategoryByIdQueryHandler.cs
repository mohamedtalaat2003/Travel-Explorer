namespace Travel_Explorer.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var spec = new CategorySpecification(request.Id);
            var category = await _unitOfWork.Repository<Category>().GenericEntitiesWithSpec(spec);

            if (category == null || category.IsDeleted)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            return _mapper.Map<CategoryDto>(category);
        }
    }
}
