using FluentValidation;
using FluentValidation.Results;
using Movies.Application.Models;
using Movies.Application.Repositories;

namespace Movies.Application.Services;

public class RatingService(IRatingRepository ratingRepository, IMovieRepository movieRepository)
    : IRatingService
{
    public async Task<bool> RateMovieAsync(
        Guid movieId,
        Guid userId,
        int rating,
        CancellationToken cancellationToken = default
    )
    {
        if (rating is <= 0 or > 10)
        {
            throw new ValidationException(
                new[]
                {
                    new ValidationFailure
                    {
                        PropertyName = "Rating",
                        ErrorMessage = "Rating must be between 1 and 10"
                    }
                }
            );
        }

        var movieExists = await movieRepository.ExistsByIdAsync(movieId, cancellationToken);
        if (!movieExists)
            return false;

        return await ratingRepository.RateMovieAsync(movieId, userId, rating, cancellationToken);
    }

    public Task<bool> DeleteRatingAsync(
        Guid movieId,
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return ratingRepository.DeleteRatingAsync(movieId, userId, cancellationToken);
    }

    public Task<IEnumerable<MovieRating>> GetUserRatingsAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        return ratingRepository.GetRatingsForUserAsync(userId, cancellationToken);
    }
}
