using Travel_Explorer.Application.DTOs.Users;

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
                user.requestToBeAuthor.ToString(),
                user.CreatedAt
            );
        }
    }
}
