using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Mapping;
using Movies.Application.Services;
using Movies.Contracts.Requests;

namespace Movies.Api.Controllers;

[Authorize]
[ApiController]
[Route("api")]
public class MoviesController(IMovieService movieService) : ControllerBase
{
    [HttpPost(ApiEndpoints.Movies.Create)]
    public async Task<IActionResult> Create(
        [FromBody] CreateMovieRequest request,
        CancellationToken cancellationToken
    )
    {
        var movie = request.MapToMovie();
        await movieService.CreateAsync(movie, cancellationToken);
        return CreatedAtAction(nameof(Get), new { idOrSlug = movie.Id }, movie.MapToResponse());
    }

    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Movies.Get)]
    public async Task<IActionResult> Get(
        [FromRoute] string idOrSlug,
        CancellationToken cancellationToken
    )
    {
        var movie = Guid.TryParse(idOrSlug, out var id)
            ? await movieService.GetByIdAsync(id, cancellationToken)
            : await movieService.GetBySlugAsync(idOrSlug, cancellationToken);

        if (movie is null)
            return NotFound();
        return Ok(movie.MapToResponse());
    }

    [AllowAnonymous]
    [HttpGet(ApiEndpoints.Movies.GetAll)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var movies = await movieService.GetAllAsync(cancellationToken);
        return Ok(movies.MapToResponse());
    }

    [HttpPut(ApiEndpoints.Movies.Update)]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMovieRequest request,
        CancellationToken cancellationToken
    )
    {
        var movie = request.MapToMovie(id);
        var updatedMovie = await movieService.UpdateAsync(movie, cancellationToken);
        if (updatedMovie == null)
            return NotFound();
        return Ok(movie.MapToResponse());
    }

    [HttpDelete(ApiEndpoints.Movies.Delete)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken
    )
    {
        var deleted = await movieService.DeleteByIdAsync(id, cancellationToken);
        if (!deleted)
            return NotFound();
        return Ok();
    }
}
