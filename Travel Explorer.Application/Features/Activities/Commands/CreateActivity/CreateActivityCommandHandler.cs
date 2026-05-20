
namespace Travel_Explorer.Application.Features.Activities.Commands.CreateActivity
{
    public class CreateActivityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<CreateActivityCommand, ActivityDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ActivityDto> Handle(
            CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var activity = _mapper.Map<Activity>(request);
            activity.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Activity>().AddAsync(activity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var spec = new ActivitySpecification(activity.Id);
            var loaded = await _unitOfWork.Repository<Activity>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<ActivityDto>(loaded);

        }
    }
}
