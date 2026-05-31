using Travel_Explorer.Application.Features.Activities;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationActivities
{
    public class GetDestinationActivitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<GetDestinationActivitiesQuery, IReadOnlyList<ActivityDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IReadOnlyList<ActivityDto>> Handle(
            GetDestinationActivitiesQuery request, CancellationToken cancellationToken)
        {
            var spec = new ActivitySpecification(destinationId: request.DestinationId);
            var activities = await _unitOfWork.Repository<Activity>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<ActivityDto>>(activities);
        }
    
    }
}
