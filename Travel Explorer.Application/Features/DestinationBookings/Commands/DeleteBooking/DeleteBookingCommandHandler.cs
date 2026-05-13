
namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler
        : IRequestHandler<DeleteBookingCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBookingCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Repository<DestinationBooking>().GetAsync(request.Id);

            if (booking == null)
                throw new NotFoundException(nameof(DestinationBooking), request.Id);

            booking.IsDeleted = true;
            booking.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        
    }
}
}
