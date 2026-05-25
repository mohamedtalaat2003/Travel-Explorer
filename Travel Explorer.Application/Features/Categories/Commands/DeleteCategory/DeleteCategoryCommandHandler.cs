namespace Travel_Explorer.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var category = await _unitOfWork.Repository<Category>().GetAsync(request.Id);

            if (category == null || category.IsDeleted)
                throw new NotFoundException(nameof(Category), request.Id);

            category.IsDeleted = true;
            category.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
