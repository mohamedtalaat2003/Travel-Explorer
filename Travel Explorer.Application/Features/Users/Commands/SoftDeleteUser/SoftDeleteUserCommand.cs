using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Application.Features.Users.Commands.SoftDeleteUser
{
    public record SoftDeleteUserCommand(int Id) : IRequest<bool>;

    public class SoftDeleteUserCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<SoftDeleteUserCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(SoftDeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(request.Id);

            if (user is null || user.IsDeleted)
                return false;

            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;
            _unitOfWork.Repository<ApplicationUser>().Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
