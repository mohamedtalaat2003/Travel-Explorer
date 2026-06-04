
namespace Travel_Explorer.Application.Features.Destinations.Commands.UpdateDestination
{
    public class UpdateDestinationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<UpdateDestinationCommand, DestinationDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<DestinationDto?> Handle(
            UpdateDestinationCommand request, CancellationToken cancellationToken)
        {
            var spec = new DestinationSpecification(request.Id);
            var destination = await _unitOfWork.Repository<Destination>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(Destination), request.Id);
            _mapper.Map(request, destination);
            destination.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            
            var reloadSpec = new DestinationSpecification(destination.Id);
            var loaded = await _unitOfWork.Repository<Destination>().GenericEntitiesWithSpec(reloadSpec);

            return _mapper.Map<DestinationDto>(loaded);
        }
    
    }
}
