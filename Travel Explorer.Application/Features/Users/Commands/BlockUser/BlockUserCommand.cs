

namespace Travel_Explorer.Application.Features.Users.Commands.BlockUser
{
    public record BlockUserCommand(int Id) : IRequest<bool>;

    public class BlockUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<BlockUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.Id);

            if (user is null || user.IsDeleted)
                return false;

            user.IsBlocked = true;
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
