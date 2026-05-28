
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.Users.Commands.RejectAuthorRequest
{
    public record RejectAuthorRequestCommand(int Id) : IRequest<bool>;

    public class RejectAuthorRequestCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RejectAuthorRequestCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(RejectAuthorRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.Id);

            if (user is null || user.IsDeleted)
                return false;

            if (user.requestToBeAuthor != RequestToBeAuthor.Pending)
                return false;

            user.requestToBeAuthor = RequestToBeAuthor.Rejected;
            user.Status = AccountStatus.Approved;
            // Role stays as "Traveler" — the user was never promoted
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
