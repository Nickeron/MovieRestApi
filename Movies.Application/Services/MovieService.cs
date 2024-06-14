using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class MovieService(IMovieRepository movieRepository, IValidator<Movie> validator)
    : IMovieService
{
    public async Task<bool> CreateAsync(
        Movie movie,
        Guid? userId = default,
        CancellationToken cancellationToken = default
    )
    {
        await validator.ValidateAndThrowAsync(movie, cancellationToken: cancellationToken);
        return await movieRepository.CreateAsync(movie, userId, cancellationToken);
    }

    public Task<Movie?> GetByIdAsync(
        Guid id,
        Guid? userId = default,
        CancellationToken cancellationToken = default
    )
    {
        return movieRepository.GetByIdAsync(id, userId, cancellationToken);
    }

    public Task<Movie?> GetBySlugAsync(
        string slug,
        Guid? userId = default,
        CancellationToken cancellationToken = default
    )
    {
        return movieRepository.GetBySlugAsync(slug, userId, cancellationToken);
    }

    public Task<IEnumerable<Movie>> GetAllAsync(
        Guid? userId = default,
        CancellationToken cancellationToken = default
    )
    {
        return movieRepository.GetAllAsync(userId, cancellationToken);
    }

    public async Task<Movie?> UpdateAsync(
        Movie movie,
        Guid? userId = default,
        CancellationToken cancellationToken = default
    )
    {
        await validator.ValidateAndThrowAsync(movie, cancellationToken);
        var movieExists = await movieRepository.ExistsByIdAsync(movie.Id, cancellationToken);
        if (!movieExists)
            return null;

        await movieRepository.UpdateAsync(movie, userId, cancellationToken);

        return movie;
    }

    public Task<bool> DeleteByIdAsync(
        Guid id,
        Guid? userId = default,
        CancellationToken cancellationToken = default
    )
    {
        return movieRepository.DeleteByIdAsync(id, userId, cancellationToken);
    }
}
