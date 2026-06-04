namespace Travel_Explorer.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null) 
                throw new ArgumentNullException(nameof(request));

            
            var duplicateSpec = new CategorySpecification(request.Name);
            var existing = await _unitOfWork.Repository<Category>().GenericEntitiesWithSpec(duplicateSpec);
            if (existing != null)
            {
                throw new BadRequestException("Category name must be unique.");
            }

            var category = _mapper.Map<Category>(request);
            category.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Category>().AddAsync(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var spec = new CategorySpecification(category.Id);
            var loaded = await _unitOfWork.Repository<Category>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<CategoryDto>(loaded);
        }
    }
}
