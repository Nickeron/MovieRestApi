namespace Movies.Application.Models;

public record GetAllMoviesOptions(
    string? Title,
    int? YearOfRelease,
    string? SortField,
    SortOrder? SortOrder
)
{
    public Guid? UserId { get; set; }
}

public enum SortOrder
{
    Unsorted,
    Ascending,
    Descending
}
