using FluentValidation;

namespace Travel_Explorer.Application.Features.Categories.Queries.GetAllCategories
{
    public class GetAllCategoriesQueryValidator : AbstractValidator<GetAllCategoriesQuery>
    {
        public GetAllCategoriesQueryValidator()
        {
            RuleFor(x => x.Params)
                .NotNull()
                .WithMessage("Parameters must not be null.");

            RuleFor(x => x.Params.PageSize)
                .LessThanOrEqualTo(50)
                .WithMessage("PageSize must not exceed 50.");

            RuleFor(x => x.Params.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageNumber must be greater than or equal to 1.");
        }
    }
}
