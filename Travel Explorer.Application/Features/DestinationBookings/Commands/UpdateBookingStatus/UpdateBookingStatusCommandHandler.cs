namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingStatus
{
    public class UpdateBookingStatusCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
                : IRequestHandler<UpdateBookingStatusCommand, DestinationBookingDto?>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<DestinationBookingDto?> Handle(
            UpdateBookingStatusCommand request, CancellationToken cancellationToken)
        {
            var spec = new DestinationBookingSpecification(request.Id);
            var booking = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(DestinationBooking), request.Id);
            booking.Status = request.Status;
            booking.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var reloadSpec = new DestinationBookingSpecification(booking.Id);
            var loaded = await _unitOfWork.Repository<DestinationBooking>().GenericEntitiesWithSpec(reloadSpec);

            return _mapper.Map<DestinationBookingDto>(loaded);
        
    }
}
}
