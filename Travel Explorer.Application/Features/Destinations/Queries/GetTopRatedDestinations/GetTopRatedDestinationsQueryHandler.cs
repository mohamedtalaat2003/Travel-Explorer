
namespace Travel_Explorer.Application.Features.Destinations.Queries.GetTopRatedDestinations
{
    public class GetTopRatedDestinationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<GetTopRatedDestinationsQuery, IReadOnlyList<DestinationDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<IReadOnlyList<DestinationDto>> Handle(
            GetTopRatedDestinationsQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationSpecification(request.Count, topRated: true);
            var destinations = await _unitOfWork.Repository<Destination>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<DestinationDto>>(destinations);
        }
    
    }
}
