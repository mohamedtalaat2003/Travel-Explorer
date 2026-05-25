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
            var userId = _currentUserService.UserId ?? 0;
            var spec = new UserProfileSpecification(userId); 
            var profile = await _unitOfWork.Repository<UserProfile>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(UserProfile), userId);

            // Update UserProfile and nested User fields using AutoMapper
            _mapper.Map(request, profile);

            _unitOfWork.Repository<UserProfile>().Update(profile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserProfileDto>(profile);
        }
    }
}
