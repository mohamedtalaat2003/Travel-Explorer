using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs.Blogs;
using Travel_Explorer.Application.Features.Destinations.Queries.SearchDestinations;

namespace Travel_Explorer.Application.Features.Blogs.Queries.Search
{
    public class Handler : IRequestHandler<SearchBlogsQuery, IReadOnlyList<BlogDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<BlogDto>> Handle(SearchBlogsQuery request, CancellationToken cancellationToken)
        {
            var spec = new BlogSpecification(request?.AutherId,request?.CategoryId);

            var blogs = await _unitOfWork.Repository<Blog>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<BlogDto>>(blogs);
        }
    }
}
