namespace Travel_Explorer.Application.Features.Users.Commands.ChangeUserRole
{
    public record ChangeUserRoleCommand(string Id, string NewRole) : IRequest<bool>;

    public class ChangeUserRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<ChangeUserRoleCommand, bool>
    {
        private static readonly string[] AllowedRoles = ["Admin", "Traveler", "Author"];
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            if (!int.TryParse(request.Id, out var userId))
                return false;

            if (string.IsNullOrWhiteSpace(request.NewRole) || !AllowedRoles.Contains(request.NewRole))
                throw new BadRequestException($"Invalid role. Allowed roles: {string.Join(", ", AllowedRoles)}.");

            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(userId);

            if (user is null || user.IsDeleted)
                return false;

            
            user.Role = request.NewRole;
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
