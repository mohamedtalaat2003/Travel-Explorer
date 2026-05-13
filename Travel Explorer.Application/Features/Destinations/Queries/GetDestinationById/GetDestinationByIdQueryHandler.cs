
namespace Travel_Explorer.Application.Features.Destinations.Queries.GetDestinationById
{
    public class GetDestinationByIdQueryHandler
        : IRequestHandler<GetDestinationByIdQuery, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetDestinationByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DestinationDto?> Handle(
            GetDestinationByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new DestinationSpecification(request.Id);
            var destination = await _unitOfWork.Repository<Destination>().GenericEntitiesWithSpec(spec);

            if (destination == null)
                throw new NotFoundException(nameof(Destination), request.Id);

            return _mapper.Map<DestinationDto>(destination);
        }
    
    }
}
