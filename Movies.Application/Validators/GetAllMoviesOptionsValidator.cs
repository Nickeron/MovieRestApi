using FluentValidation;
using Movies.Application.Models;

namespace Movies.Application.Validators;

public class GetAllMoviesOptionsValidator : AbstractValidator<GetAllMoviesOptions>
{
    private static readonly string[] AcceptableSortFields = ["title", "year"];

    public GetAllMoviesOptionsValidator()
    {
        RuleFor(movie => movie.YearOfRelease).LessThanOrEqualTo(DateTime.UtcNow.Year);
        RuleFor(movie => movie.SortField)
            .Must(sortField =>
                sortField is null
                || AcceptableSortFields.Contains(sortField, StringComparer.OrdinalIgnoreCase)
            )
            .WithMessage("You can only sort by 'title' or 'year'");

        RuleFor(movie => movie.Page).GreaterThanOrEqualTo(1);
        RuleFor(movie => movie.PageSize)
            .InclusiveBetween(1, 25)
            .WithMessage("You can get between 1 and 25 movies per page");
    }
}
