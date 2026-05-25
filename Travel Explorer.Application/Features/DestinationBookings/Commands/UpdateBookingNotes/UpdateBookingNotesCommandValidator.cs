using FluentValidation;

namespace Travel_Explorer.Application.Features.DestinationBookings.Commands.UpdateBookingNotes
{
    public class UpdateBookingNotesCommandValidator : AbstractValidator<UpdateBookingNotesCommand>
    {
        public UpdateBookingNotesCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Booking ID is required and must be greater than 0.");

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes must not exceed 1000 characters.");
        }
    }
}
