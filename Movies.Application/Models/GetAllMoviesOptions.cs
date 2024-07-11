namespace Movies.Application.Models;

public record GetAllMoviesOptions(string? Title, int? YearOfRelease)
{
    public Guid? UserId { get; set; }
}
