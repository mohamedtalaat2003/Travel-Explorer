using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Blogs.Commands.DeleteBlog
{
    public class DeleteBlogCommandHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : IRequestHandler<DeleteBlogCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<bool> Handle(DeleteBlogCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var blog = await _unitOfWork.Repository<Blog>().GetAsync(request.Id);

            blog.AuthorId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("You are not authorized to delete this blog.");

            if (blog == null || blog.IsDeleted)
            {
                throw new NotFoundException(nameof(Blog), request.Id);
            }



            blog.IsDeleted = true;
            blog.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
