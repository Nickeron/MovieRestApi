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
    }
}
