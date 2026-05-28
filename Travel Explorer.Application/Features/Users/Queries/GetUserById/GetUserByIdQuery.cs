using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Domain.Entities;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Application.Features.Users.Queries.GetUserById
{
    public record GetUserByIdQuery(int Id) : IRequest<UserDetailsDto?>;

    public class GetUserByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetUserByIdQuery, UserDetailsDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<UserDetailsDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new UserSpecification(request.Id);
            var user = await _unitOfWork.Repository<ApplicationUser>().GenericEntitiesWithSpec(spec);

            if (user is null)
                return null;

            return new UserDetailsDto(
                user.Id,
                user.UserName ?? string.Empty,
                user.Email,
                user.Gender,
                user.IsBlocked,
                user.Role,
                user.Status.ToString(),
                user.CreatedAt
            );
        }
    }
}
