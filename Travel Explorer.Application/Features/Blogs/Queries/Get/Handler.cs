using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs.Blogs;

namespace Travel_Explorer.Application.Features.Blogs.Queries.Get
{
    public class Handler : IRequestHandler<GetBlogQuery, BlogDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BlogDto> Handle(GetBlogQuery request, CancellationToken cancellationToken)
        {
            if(request.Id <= 0)
                throw new ArgumentException("Invalid blog ID.");

            var spec = new BlogSpecification(request.Id);
            var blog = await _unitOfWork.Repository<Blog>().GenericEntitiesWithSpec(spec);

            if(!blog.IsPublished) 
                throw new KeyNotFoundException("Blog not published.");

            if (blog == null)
                throw new KeyNotFoundException("Blog not found.");

            return _mapper.Map<BlogDto>(blog);
        }
    }
}
