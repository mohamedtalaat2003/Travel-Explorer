using Travel_Explorer.Application.Features.Activities;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationActivities
{
    public class GetDestinationActivitiesQueryHandler
        : IRequestHandler<GetDestinationActivitiesQuery, IReadOnlyList<ActivityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDestinationActivitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<ActivityDto>> Handle(
            GetDestinationActivitiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new ActivitySpecification(request.DestinationId);
            var activities = await _unitOfWork.Repository<Activity>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<ActivityDto>>(activities);
        }
    
    }
}
