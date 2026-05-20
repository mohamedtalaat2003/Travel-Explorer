using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Profiles;

namespace Travel_Explorer.Application.Features.Profiles.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<UserProfileDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? 0;
            var spec = new UserProfileSpecification(userId);
            var profile = await _unitOfWork.Repository<UserProfile>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(UserProfile), userId);
            return _mapper.Map<UserProfileDto>(profile);
        }
    }
}
