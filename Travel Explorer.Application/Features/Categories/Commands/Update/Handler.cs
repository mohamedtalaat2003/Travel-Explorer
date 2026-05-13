using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travel_Explorer.Application.Features.Categories.Commands.Update
{
    public class Handler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public Handler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var category = _mapper.Map<Category>(request);

            try
            {
                category.UpdatedAt = DateTime.UtcNow;

                 _unitOfWork.Repository<Category>().Update(category);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }catch (Exception ex)
            {
                throw new Exception("Exception occure when try to update");
            }

            var spec = new CategorySpecification(request.Id);
            var loaded = _unitOfWork.Repository<Category>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<CategoryDto>(loaded);
        }
    }
}
