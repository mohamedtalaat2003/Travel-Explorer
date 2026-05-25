using FluentValidation;

namespace Travel_Explorer.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Category ID must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Category name is required.")
                .Length(2, 100)
                .WithMessage("Category name must be between 2 and 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.IconUrl)
                .MaximumLength(500)
                .WithMessage("IconUrl must not exceed 500 characters.");
        }
    }
}
