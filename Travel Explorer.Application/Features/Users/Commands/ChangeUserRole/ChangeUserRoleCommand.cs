using Microsoft.AspNetCore.Identity;

namespace Travel_Explorer.Application.Features.Users.Commands.ChangeUserRole
{
    public record ChangeUserRoleCommand(string Id, string NewRole) : IRequest<bool>;
  
    public class ChangeUserRoleCommandHandler : IRequestHandler<ChangeUserRoleCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;

      
        public ChangeUserRoleCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(ChangeUserRoleCommand request, CancellationToken cancellationToken)
        {
            
            var user = await _userManager.FindByIdAsync(request.Id.ToString());

            if (user is null || user.IsDeleted)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!removeResult.Succeeded)
                return false;

           
            var addResult = await _userManager.AddToRoleAsync(user, request.NewRole);

            if (!addResult.Succeeded)
                return false;

            user.Role = request.NewRole;
            await _userManager.UpdateAsync(user);

            return true;
        }
    }
}