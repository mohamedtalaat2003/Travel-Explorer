using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Application.Features.Blogs;
using Travel_Explorer.Application.Features.Blogs.Commands.Add;

namespace Travel_Explorer.Application.Features.Categories.Commands.Create
{
    public class Handler : IRequestHandler<CreateCategoryCommand, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            if(request == null) 
                throw new ArgumentNullException(nameof(request));

            var category = _mapper.Map<Category>(request);
            var check = await _unitOfWork.Repository<Category>().GetAllAsync();
                
            foreach(var item in check)
            {
                if (item.Name == request.Name)
                    throw new Exception("Name must be unique");
            }
            
            try
            {
                category.CreatedAt = DateTime.UtcNow;

                await _unitOfWork.Repository<Category>().AddAsync(category);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

            } catch (Exception ex)
            {
                throw new Exception("Exception occure when try to create");
            }

            var spec = new CategorySpecification(category.Id);
            var loaded =await _unitOfWork.Repository<Category>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<CategoryDto>(loaded);

        }
    }
}
