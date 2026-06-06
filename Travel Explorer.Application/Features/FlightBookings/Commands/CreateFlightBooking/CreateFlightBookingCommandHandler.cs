using Travel_Explorer.Application.DTOs.Flights.Bookings;
using Microsoft.EntityFrameworkCore;
using Travel_Explorer.Application.Common;
using Travel_Explorer.Domain.Enums;

namespace Travel_Explorer.Application.Features.FlightBookings.Commands.CreateFlightBooking
{
    public class CreateFlightBookingCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<CreateFlightBookingCommand, FlightBookingDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ICurrentUserService _currentUserService = currentUserService;

        public async Task<FlightBookingDto> Handle(CreateFlightBookingCommand request, CancellationToken cancellationToken)
        {
            var flightSchedule = await _unitOfWork.Repository<FlightSchedule>().GetAsync(request.FlightScheduleId) ?? throw new NotFoundException(nameof(FlightSchedule), request.FlightScheduleId);
            decimal ticketPrice;

            switch (request.Class)
            {
                case FlightClass.Economy:
                    if (flightSchedule.AvailableEconomySeats < request.NumberOfPassengers)
                        throw new BadRequestException("Not enough Economy seats available on this flight.");
                    ticketPrice = flightSchedule.EconomyPrice;
                    flightSchedule.AvailableEconomySeats -= request.NumberOfPassengers;
                    break;

                case FlightClass.Business:
                    if (flightSchedule.AvailableBusinessSeats < request.NumberOfPassengers)
                        throw new BadRequestException("Not enough Business seats available on this flight.");
                    ticketPrice = flightSchedule.BusinessPrice;
                    flightSchedule.AvailableBusinessSeats -= request.NumberOfPassengers;
                    break;

                case FlightClass.FirstClass:
                    if (flightSchedule.AvailableFirstClassSeats < request.NumberOfPassengers)
                        throw new BadRequestException("Not enough First Class seats available on this flight.");
                    ticketPrice = flightSchedule.FirstClassPrice;
                    flightSchedule.AvailableFirstClassSeats -= request.NumberOfPassengers;
                    break;

                default:
                    throw new BadRequestException("Invalid flight class.");
            }

            decimal totalPrice = ticketPrice * request.NumberOfPassengers;

            var booking = _mapper.Map<FlightBooking>(request);
            booking.UserId = _currentUserService.UserId ??0;
            booking.TotalPrice = totalPrice;
            booking.Status = BookingStatus.Pending;
            booking.CreatedAt = DateTime.UtcNow;

            _unitOfWork.Repository<FlightSchedule>().Update(flightSchedule);
            await _unitOfWork.Repository<FlightBooking>().AddAsync(booking);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new BadRequestException("The flight seats were booked by another user. Please try again.");
            }

            
            var spec = new FlightBookingSpecification(booking.Id);
            var loadedBooking = await _unitOfWork.Repository<FlightBooking>().GenericEntitiesWithSpec(spec);

            return _mapper.Map<FlightBookingDto>(loadedBooking);
        }
    }
}
