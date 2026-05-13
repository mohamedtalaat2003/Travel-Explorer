
namespace Travel_Explorer.Application.Features.Activities.Commands.UpdateActivity
{
    public class UpdateActivityCommandHandler
        : IRequestHandler<UpdateActivityCommand, ActivityDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateActivityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActivityDto?> Handle(
            UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var spec = new ActivitySpecification(request.Id);
            var activity = await _unitOfWork.Repository<Activity>().GenericEntitiesWithSpec(spec);

            if (activity == null)
                throw new NotFoundException(nameof(Activity), request.Id);

            _mapper.Map(request, activity);
            activity.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var reloadSpec = new ActivitySpecification(activity.Id);
            var loaded = await _unitOfWork.Repository<Activity>().GenericEntitiesWithSpec(reloadSpec);

            return _mapper.Map<ActivityDto>(loaded);
        
    }
}
}
