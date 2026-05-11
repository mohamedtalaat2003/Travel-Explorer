using AutoMapper;
using Travel_Explorer.Application.DTOs.Profiles;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Application.Features.Profiles.Queries.GetUserProfile
{
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserProfileQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileDto?> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var spec = new UserProfileSpecification(request.UserId);
            var profile = await _unitOfWork.Repository<UserProfile>().GenericEntitiesWithSpec(spec);

            if (profile == null)
                return null;

            return _mapper.Map<UserProfileDto>(profile);
        }
    }
}
