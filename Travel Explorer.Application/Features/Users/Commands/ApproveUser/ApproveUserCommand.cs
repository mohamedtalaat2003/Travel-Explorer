
using Travel_Explorer.Domain.Enums;


namespace Travel_Explorer.Application.Features.Users.Commands.ApproveUser
{
    public record ApproveUserCommand(int Id) : IRequest<bool>;

    public class ApproveUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ApproveUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(ApproveUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.Id);

            if (user is null || user.IsDeleted)
                return false;

            user.Status = AccountStatus.Approved;
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
