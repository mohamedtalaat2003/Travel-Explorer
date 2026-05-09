
namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingNotes
{
    public class UpdateBookingNotesCommandHandler
        : IRequestHandler<UpdateBookingNotesCommand, DestinationBookingDto?>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateBookingNotesCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<DestinationBookingDto?> Handle(
            UpdateBookingNotesCommand request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec);

            if (booking == null)
                return null;

            booking.Notes = request.Notes;
            booking.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var reloadSpec = new DestinationBookingSpecification(booking.Id);
            var loaded = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(reloadSpec);

            return _mapper.Map<DestinationBookingDto>(loaded);
        
    }
}
}
