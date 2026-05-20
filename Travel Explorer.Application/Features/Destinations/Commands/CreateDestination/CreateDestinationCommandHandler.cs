
namespace Travel_Explorer.Application.Features.Destinations.Commands.CreateDestination
{
    public class CreateDestinationCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<CreateDestinationCommand, DestinationDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<DestinationDto> Handle(
            CreateDestinationCommand request, CancellationToken cancellationToken)
        {
            var destination = _mapper.Map<Destination>(request);
            destination.AverageRating = 0;
            destination.ReviewCount = 0;
            destination.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Destination>().AddAsync(destination);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // Reload with includes for the response
            var spec = new DestinationSpecification(destination.Id);
            var loaded = await _unitOfWork.Repository<Destination>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<DestinationDto>(loaded);
        }
    
    }
}
