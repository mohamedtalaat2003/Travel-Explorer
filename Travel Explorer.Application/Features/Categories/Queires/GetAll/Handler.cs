using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Categories.Queires.GetAll
{
    public class Handler : IRequestHandler<GetAllCategoriesQuery, PaginatedResult<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            if(request == null) 
                throw new ArgumentException(nameof(request));

            var spec = new GetAllCategorySpecification(request.PageNumber,request.PageSize);
            var count = await _unitOfWork.Repository<Category>().CountAsync(spec);

            if (count <= 0)
                throw new ArgumentException("There is no Categories");

            var categories = await _unitOfWork.Repository<Category>().ListSpecAsync(spec);


            var dtos = _mapper.Map<IReadOnlyList<CategoryDto>>(categories);

            return new PaginatedResult<CategoryDto>(dtos, count, request.PageNumber, request.PageSize); 

            
        }
    }
}
