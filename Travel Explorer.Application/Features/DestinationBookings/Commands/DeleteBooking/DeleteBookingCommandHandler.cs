
namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.DeleteBooking
{
    public class DeleteBookingCommandHandler(IUnitOfWork unitOfWork)
                : IRequestHandler<DeleteBookingCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(
            DeleteBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Repository<DestinationBooking>().GetAsync(request.Id) ?? throw new NotFoundException(nameof(DestinationBooking), request.Id);
            booking.IsDeleted = true;
            booking.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return true;
        
    }
}
}
