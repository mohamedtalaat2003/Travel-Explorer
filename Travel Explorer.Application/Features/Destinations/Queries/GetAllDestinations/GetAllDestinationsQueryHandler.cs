using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.Destinations.Queries.GetAllDestinations
{
    public class GetAllDestinationsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<GetAllDestinationsQuery, PaginatedResult<DestinationDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<PaginatedResult<DestinationDto>> Handle(
            GetAllDestinationsQuery request, CancellationToken cancellationToken)
        {
            var p = request.Params;
            var spec = new DestinationSpecification(p);

            
            var totalCount   = await _unitOfWork.Repository<Destination>().CountAsync(spec);
            var destinations = await _unitOfWork.Repository<Destination>().ListSpecAsync(spec);

            var dtos = _mapper.Map<IReadOnlyList<DestinationDto>>(destinations);
            return new PaginatedResult<DestinationDto>(dtos, totalCount, p.PageNumber, p.PageSize);
        }
    }
}
