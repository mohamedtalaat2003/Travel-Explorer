
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.Users.Commands.RequestAuthorRole
{
    /// <summary>
    /// Command for a logged-in user to request upgrading their role to Author.
    /// </summary>
    public record RequestAuthorRoleCommand(int UserId) : IRequest<bool>;

    public class RequestAuthorRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RequestAuthorRoleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(RequestAuthorRoleCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.UserId);

            if (user is null || user.IsDeleted)
                return false;

            // Only allow if not already pending or approved
            if (user.requestToBeAuthor == RequestToBeAuthor.Pending)
                return false; // Already has a pending request

            if (user.Role == "Author")
                return false; // Already an Author

            user.requestToBeAuthor = RequestToBeAuthor.Pending;
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
