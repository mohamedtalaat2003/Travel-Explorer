
namespace Travel_Explorer.Application.Features.Destinations.Queries.SearchDestinations
{
    public class SearchDestinationsQueryHandler
        : IRequestHandler<SearchDestinationsQuery, IReadOnlyList<DestinationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchDestinationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<DestinationDto>> Handle(
            SearchDestinationsQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationSpecification(
                request.Keyword, request.Location, request.MinPrice, request.MaxPrice, request.CategoryId);

            var destinations = await _unitOfWork.Repository<Destination>().ListSpecAsync(spec);

            return _mapper.Map<IReadOnlyList<DestinationDto>>(destinations);
        }
    
    }
}
