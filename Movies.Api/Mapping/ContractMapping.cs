using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

namespace Movies.Api.Mapping;

public static class ContractMapping
{
    public static Movie MapToMovie(this CreateMovieRequest request) =>
        new()
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()
        };

    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id) =>
        new()
        {
            Id = id,
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList()
        };

    public static MovieResponse MapToResponse(this Movie movie) =>
        new()
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            Rating = movie.Rating,
            UserRating = movie.UserRating,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres.ToList()
        };

    private static MovieRatingResponse MapToResponse(this MovieRating rating) =>
        new()
        {
            MovieId = rating.MovieId,
            Slug = rating.Slug,
            Rating = rating.Rating,
        };

    public static MoviesResponse MapToResponse(this IEnumerable<Movie> movies) =>
        new() { Items = movies.Select(MapToResponse) };

    public static IEnumerable<MovieRatingResponse> MapToResponse(
        this IEnumerable<MovieRating> ratings
    ) => ratings.Select(MapToResponse);

    public static GetAllMoviesOptions MapToOptions(this GetAllMoviesRequest request) =>
        new(request.Title, request.YearOfRelease);

    public static GetAllMoviesOptions WithUser(this GetAllMoviesOptions options, Guid? userId)
    {
        options.UserId = userId;
        return options;
    }
}
