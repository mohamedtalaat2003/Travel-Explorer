using FluentValidation;

namespace Travel_Explorer.Application.Features.Blogs.Commands.UpdateBlog
{
    public class UpdateBlogCommandValidator : AbstractValidator<UpdateBlogCommand>
    {
        public UpdateBlogCommandValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Blog ID must be greater than 0.");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Blog title is required.")
                .Length(5, 250)
                .WithMessage("Title must be between 5 and 250 characters.");

            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("Content is required.");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500)
                .WithMessage("ImageUrl must not exceed 500 characters.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .When(x => x.CategoryId.HasValue)
                .WithMessage("CategoryId must be greater than 0 if provided.");
        }
    }
}
