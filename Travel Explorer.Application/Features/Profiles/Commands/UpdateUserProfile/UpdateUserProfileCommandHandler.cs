using Travel_Explorer.Application.Common;
using Travel_Explorer.Application.DTOs.Profiles;

namespace Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<UpdateUserProfileCommand, UserProfileDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<UserProfileDto?> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId ?? throw new UnauthorizedAccessException();

            var spec = new UserProfileSpecification(userId);
            var profile = await _unitOfWork.Repository<UserProfile>().GenericEntitiesWithSpec(spec);

            var isNew = profile is null;
            if (profile is null)
            {
                
                
                var user = await _unitOfWork.Repository<ApplicationUser>().GetAsync(userId)
                    ?? throw new NotFoundException(nameof(ApplicationUser), userId);

                profile = new UserProfile { UserId = userId, User = user };
            }

            
            _mapper.Map(request, profile);
            profile.UserId = userId;

            
            if (profile.User is not null && !string.IsNullOrWhiteSpace(profile.User.Email))
                profile.User.NormalizedEmail = profile.User.Email.ToUpperInvariant();

            
            if (profile.DateOfBirth.HasValue)
                profile.DateOfBirth = DateTime.SpecifyKind(profile.DateOfBirth.Value, DateTimeKind.Utc);

            if (isNew)
                await _unitOfWork.Repository<UserProfile>().AddAsync(profile);
            else
                _unitOfWork.Repository<UserProfile>().Update(profile);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserProfileDto>(profile);
        }
    }
}
