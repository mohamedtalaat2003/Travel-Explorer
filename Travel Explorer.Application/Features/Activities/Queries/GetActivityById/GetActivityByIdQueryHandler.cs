
namespace Travel_Explorer.Application.Features.Activities.Queries.GetActivityById
{
    public class GetActivityByIdQueryHandler
        : IRequestHandler<GetActivityByIdQuery, ActivityDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetActivityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ActivityDto?> Handle(
            GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new ActivitySpecification(request.Id);
            var activity = await _unitOfWork.Repository<Activity>().GenericEntitiesWithSpec(spec);

            if (activity == null)
                return null;

            return _mapper.Map<ActivityDto>(activity);

        }
    }
}
