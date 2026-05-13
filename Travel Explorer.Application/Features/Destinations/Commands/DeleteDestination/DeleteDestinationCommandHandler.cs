
namespace Travel_Explorer.Application.Features.Destinations.Commands.DeleteDestination
{
    public class DeleteDestinationCommandHandler
        : IRequestHandler<DeleteDestinationCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDestinationCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteDestinationCommand request, CancellationToken cancellationToken)
        {
            var destination = await _unitOfWork.Repository<Destination>().GetAsync(request.Id);

            if (destination == null)
                throw new NotFoundException(nameof(Destination), request.Id);

            destination.IsDeleted = true;
            destination.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        }
    
    }
}
