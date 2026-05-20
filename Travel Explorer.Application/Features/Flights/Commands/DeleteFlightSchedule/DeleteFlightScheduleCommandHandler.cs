namespace Travel_Explorer.Application.Features.Flights.Commands.DeleteFlightSchedule
{
    public class DeleteFlightScheduleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteFlightScheduleCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(DeleteFlightScheduleCommand request, CancellationToken cancellationToken)
        {
            var spec = new FlightScheduleSpecification(request.Id);
            var flightSchedule = await _unitOfWork.Repository<FlightSchedule>().GenericEntitiesWithSpec(spec) ?? throw new NotFoundException(nameof(FlightSchedule), request.Id);
            flightSchedule.IsDeleted = true;
            flightSchedule.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<FlightSchedule>().Update(flightSchedule);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
