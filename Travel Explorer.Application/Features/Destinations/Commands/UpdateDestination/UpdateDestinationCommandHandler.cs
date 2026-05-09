
namespace Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination
{
    public class UpdateDestinationCommandHandler
        : IRequestHandler<UpdateDestinationCommand, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateDestinationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DestinationDto?> Handle(
            UpdateDestinationCommand request, CancellationToken cancellationToken)
        {
            var spec = new DestinationSpecification(request.Id);
            var destination = await _unitOfWork.Repository<Destination>().GenericEntitiesWithSpec(spec);

            if (destination == null)
                return null;

            _mapper.Map(request, destination);
            destination.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Reload with includes for the response
            var reloadSpec = new DestinationSpecification(destination.Id);
            var loaded = await _unitOfWork.Repository<Destination>().GenericEntitiesWithSpec(reloadSpec);

            return _mapper.Map<DestinationDto>(loaded);
        }
    
    }
}
