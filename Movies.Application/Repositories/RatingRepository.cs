using Dapper;
using Movies.Application.Database;

namespace Movies.Application.Repositories;

public class RatingRepository(IDbConnectionFactory dbConnectionFactory) : IRatingRepository
{
    public async Task<float?> GetRatingAsync(
        Guid movieId,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<float>(
            new CommandDefinition(
                """
                 select round(avg(r.rating), 1)
                 from ratings r
                 where movieid = @movieid
                """,
                new { movieId },
                cancellationToken: cancellationToken
            )
        );
    }

    public async Task<(float? Rating, int? UserRating)> GetUserRatingAsync(
        Guid movieId,
        Guid userId,
        CancellationToken cancellationToken = default
    )
    {
        using var connection = await dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.QuerySingleOrDefaultAsync<(float?, int?)>(
            new CommandDefinition(
                """
                 select round(avg(r.rating), 1),
                    (select rating from rating
                        where movieid = @movieid
                          and userid = @userid
                        limit 1)
                 from ratings r
                 where movieid = @movieid
                """,
                new { movieId, userId },
                cancellationToken: cancellationToken
            )
        );
    }
}
