
namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationById
{
    public class GetDestinationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<GetDestinationByIdQuery, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<DestinationDto?> Handle(
            GetDestinationByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationSpecification(request.Id);
            var destination = await _unitOfWork.Repository<Destination>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(Destination), request.Id);
            return _mapper.Map<DestinationDto>(destination);
        }
    
    }
}
