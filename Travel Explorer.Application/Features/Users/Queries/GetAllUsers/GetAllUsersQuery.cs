
using Travel_Explorer.Application.Common.Parameters;
using Travel_Explorer.Application.DTOs.Users;


namespace Travel_Explorer.Application.Features.Users.Queries.GetAllUsers
{
    public record GetAllUsersQuery(UserSpecParams Parameters) : IRequest<(IEnumerable<UserDto> Users, int TotalCount)>;

    public class GetAllUsersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllUsersQuery, (IEnumerable<UserDto> Users, int TotalCount)>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<(IEnumerable<UserDto> Users, int TotalCount)> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var spec = new UserSpecification(request.Parameters);
            var usersList = await _unitOfWork.Repository<ApplicationUser>().ListSpecAsync(spec);
            var totalCount = await _unitOfWork.Repository<ApplicationUser>().CountAsync(spec);

            var usersDto = usersList.Select(u => new UserDto(
                u.Id,
                u.UserName ?? string.Empty,
                u.Email,
                u.Gender,
                u.IsBlocked,
                u.Status.ToString()
            ));

            return (usersDto, totalCount);
        }
    }
}
