using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetAllDestinations
{
    public class GetAllDestinationsQueryHandler
        : IRequestHandler<GetAllDestinationsQuery, PaginatedResult<DestinationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDestinationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<DestinationDto>> Handle(
            GetAllDestinationsQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationSpecification(request.PageNumber, request.PageSize);
            
            var totalCount = await _unitOfWork.Repository<Destination>().CountAsync(spec);
            var destinations = await _unitOfWork.Repository<Destination>().ListSpecAsync(spec);

            var dtos = _mapper.Map<IReadOnlyList<DestinationDto>>(destinations);

            return new PaginatedResult<DestinationDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    
    }
}
