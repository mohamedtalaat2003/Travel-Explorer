using AutoMapper;
using Travel_Explorer.Application.DTOs.Profiles;
using Travel_Explorer.Domain.Interfaces;

namespace Travel_Explorer.Application.Features.Profiles.Commands.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserProfileDto?> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var spec = new UserProfileSpecification(request.UserId); 
            var profile = await _unitOfWork.Repository<UserProfile>().GenericEntitiesWithSpec(spec);

            if (profile == null)
            {
                // Optionally create profile if it doesn't exist? 
                // But usually it's created on registration. 
                // For now, let's just return null or handle appropriately.
                return null;
            }

            // Update UserProfile and nested User fields using AutoMapper
            _mapper.Map(request, profile);

            _unitOfWork.Repository<UserProfile>().Update(profile);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return _mapper.Map<UserProfileDto>(profile);
        }
    }
}
