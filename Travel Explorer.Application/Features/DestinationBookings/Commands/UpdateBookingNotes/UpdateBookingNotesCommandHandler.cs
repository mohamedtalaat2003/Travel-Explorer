using Travel_Explorer.Application.Common;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingNotes
{
    public class UpdateBookingNotesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
                : IRequestHandler<UpdateBookingNotesCommand, DestinationBookingDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<DestinationBookingDto?> Handle(
            UpdateBookingNotesCommand request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(DestinationBooking), request.Id);

            
            if (!_currentUserService.IsAdmin && booking.UserId != _currentUserService.UserId)
                throw new ForbiddenAccessException("You are not authorized to update this booking.");

            booking.Notes = request.Notes;
            booking.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var reloadSpec = new DestinationBookingSpecification(booking.Id);
            var loaded = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(reloadSpec);

            return _mapper.Map<DestinationBookingDto>(loaded);
        
    }
}
}
