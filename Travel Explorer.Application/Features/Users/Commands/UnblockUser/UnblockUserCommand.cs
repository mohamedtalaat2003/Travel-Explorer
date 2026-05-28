
namespace Travel_Explorer.Application.Features.Users.Commands.UnblockUser
{
    public record UnblockUserCommand(int Id) : IRequest<bool>;

    public class UnblockUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UnblockUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(UnblockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.Id);

            if (user is null || user.IsDeleted)
                return false;

            user.IsBlocked = false;
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
