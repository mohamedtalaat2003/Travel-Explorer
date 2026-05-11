
namespace Travel_Explorer.Application.Features.Activities.Queries.GetAllActivities
{
    public class GetAllActivitiesQueryHandler
        : IRequestHandler<GetAllActivitiesQuery, IReadOnlyList<ActivityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllActivitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ActivityDto>> Handle(
            GetAllActivitiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new ActivitySpecification(request.destinationId);
            var activities = await _unitOfWork.Repository<Activity>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<ActivityDto>>(activities);

        }
    }
}
