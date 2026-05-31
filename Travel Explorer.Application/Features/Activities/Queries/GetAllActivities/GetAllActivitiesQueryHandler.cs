

using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Activities.Queries.GetAllActivities
{
    public class GetAllActivitiesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        : IRequestHandler<GetAllActivitiesQuery, PaginatedResult<ActivityDto>> 
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<ActivityDto>> Handle(
            GetAllActivitiesQuery request, CancellationToken cancellationToken)
        {
           
            var dataSpec = new ActivitySpecification(request.destinationId, request.PageNumber, request.PageSize);

            var totalCount = await _unitOfWork.Repository<Activity>().CountAsync(dataSpec);

           
            var activities = await _unitOfWork.Repository<Activity>().ListSpecAsync(dataSpec);

           
            var dtos = _mapper.Map<IReadOnlyList<ActivityDto>>(activities);

            return new PaginatedResult<ActivityDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}