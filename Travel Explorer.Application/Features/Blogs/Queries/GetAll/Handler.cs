using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Application.Features.Activities.Queries.GetAllActivities;
using Travel_Explorer.Application.Features.Destinations.Queries.GetAllDestinations;

namespace Travel_Explorer.Application.Features.Blogs.Queries.GetAll
{
    public class Handler : IRequestHandler<GetAllBlogsQuery, PaginatedResult<BlogDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<BlogDto>> Handle(GetAllBlogsQuery request, CancellationToken cancellationToken)
        {
            
            var spec = new GetAllBlogsSpecification(request.PageNumber, request.PageSize);

            var totalCount = await _unitOfWork.Repository<Blog>().CountAsync(spec);

            if (totalCount <= 0) 
                throw new Exception("There is no Blogs");

            var blogs = await _unitOfWork.Repository<Blog>().ListSpecAsync(spec);

            var dtos = _mapper.Map<IReadOnlyList<BlogDto>>(blogs);


            return new PaginatedResult<BlogDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }

    }
}
