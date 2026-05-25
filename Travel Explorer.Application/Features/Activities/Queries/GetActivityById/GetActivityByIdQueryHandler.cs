
namespace Travel_Explorer.Application.Features.Activities.Queries.GetActivityById
{
    public class GetActivityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<GetActivityByIdQuery, ActivityDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<ActivityDto?> Handle(
            GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ActivitySpecification(request.Id);
            var activity = await _unitOfWork.Repository<Activity>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(Activity), request.Id);
            return _mapper.Map<ActivityDto>(activity);

        }
    }
}
