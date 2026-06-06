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
            var profile = await _unitOfWork.Repository<UserProfile>().GenericEntitiesWithSpec(spec);
            if (profile == null)
            {
                var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(userId)
                    ?? throw new NotFoundException(nameof(ApplicationUser), userId);

                profile = new UserProfile { UserId = userId, User = user };
                await _unitOfWork.Repository<UserProfile>().AddAsync(profile);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            return _mapper.Map<UserProfileDto>(profile);
        }
    }
}
