using FluentValidation;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingStatus
{
    public class UpdateBookingStatusCommandValidator : AbstractValidator<UpdateBookingStatusCommand>
    {
        public UpdateBookingStatusCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Booking ID is required and must be greater than 0.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid booking status.");
        }
    }
}
