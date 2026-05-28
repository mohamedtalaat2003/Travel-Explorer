
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.Users.Commands.RejectUser
{
    public record RejectUserCommand(int Id) : IRequest<bool>;

    public class RejectUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RejectUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(RejectUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.Id);

            if (user is null || user.IsDeleted)
                return false;

            user.Status = AccountStatus.Rejected;
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
