using FluentValidation;

namespace Travel_Explorer.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
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
