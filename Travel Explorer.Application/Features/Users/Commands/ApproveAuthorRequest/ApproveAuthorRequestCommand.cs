
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.Users.Commands.ApproveAuthorRequest
{
    public record ApproveAuthorRequestCommand(int Id) : IRequest<bool>;

    public class ApproveAuthorRequestCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ApproveAuthorRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(ApproveAuthorRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.Id);

            if (user is null || user.IsDeleted)
                return false;

            if (user.requestToBeAuthor != RequestToBeAuthor.Pending)
                return false;

            user.requestToBeAuthor = RequestToBeAuthor.Approved;
            user.Role = "Author";
            user.Status = AccountStatus.Approved;
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
