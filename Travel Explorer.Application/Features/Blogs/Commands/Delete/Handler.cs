using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Blogs.Commands.Delete
{
    public class Handler : IRequestHandler<DeleteBlogCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public Handler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            if(request == null)
                throw new ArgumentNullException(nameof(request));

            var blog = await _unitOfWork.Repository<Blog>().GetAsync(request.Id);
            blog.AuthorId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

            if (blog == null || blog.IsDeleted)
                return false;

            try
            {
                blog.IsDeleted = true;
                await _unitOfWork.Repository<Blog>().Delete(blog);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return true;

            }
            catch (Exception ex)
            {
                //Global Exception Handling
                // Log the exception (ex) here using your logging framework
                throw new Exception("Exception occure when try to delete");
            }
         
        }
    }
}
