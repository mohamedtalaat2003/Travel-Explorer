
using Travel_Explorer.Application.DTOs.Users;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.Users.Queries.GetPendingAuthorRequests
{
    public record GetPendingAuthorRequestsQuery() : IRequest<IEnumerable<UserDto>>;

    public class GetPendingAuthorRequestsQueryHandler(IUnitOfWork unitOfWork)
        : IRequestHandler<GetPendingAuthorRequestsQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<UserDto>> Handle(GetPendingAuthorRequestsQuery request, CancellationToken cancellationToken)
        {
            var spec = new BaseSpecification<ApplicationUser>(
                u => !u.IsDeleted && u.requestToBeAuthor == RequestToBeAuthor.Pending);

            var users = await _unitOfWork.Repository<ApplicationUser>().ListSpecAsync(spec);

            return users.Select(u => new UserDto(
                u.Id,
                u.UserName ?? string.Empty,
                u.Email,
                u.Gender,
                u.IsBlocked,
                u.Role,
                u.Status.ToString(),
                u.requestToBeAuthor.ToString()
            ));
        }
    }
}
