namespace Travel_Explorer.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var existingCategory = await _unitOfWork.Repository<Category>().GetAsync(request.Id);
            if (existingCategory == null || existingCategory.IsDeleted)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }

            // Map updated fields from request onto existing entity
            _mapper.Map(request, existingCategory);
            existingCategory.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<Category>().Update(existingCategory);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var spec = new CategorySpecification(request.Id);
            var loaded = await _unitOfWork.Repository<Category>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<CategoryDto>(loaded);
        }
    }
}
