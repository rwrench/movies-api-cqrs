using FluentValidation;
using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Application.Validators
{
    public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
    {
        static readonly string[] AcceptableSortFields = new string[]
        {
           "title", "yearofrelease"
        };
        public GetAllMoviesOptionsValidator()
        {
            RuleFor(x => x.YearOfRelease)
               .LessThanOrEqualTo(DateTime.UtcNow.Year);

            RuleFor(x => x.SortField)
               .Must(x => x is null || AcceptableSortFields.Contains(x,
               StringComparer.OrdinalIgnoreCase)
                ).WithMessage($"Sort field must be one of: {string.Join(", ", AcceptableSortFields)}");

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1)
                .When(x => x.Page.HasValue);

            RuleFor(x => x.PageSize)
               .InclusiveBetween(1, 25)
               .WithMessage("Page size must be between 1 and 25");
        }
    }
}
