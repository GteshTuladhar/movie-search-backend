using MovieSearchCase.Domain.Entities;
using MovieSearchCase.Domain.Models;

namespace MovieSearchCase.Domain.Interfaces.Services;

/// <summary>
/// TMDB-backed movie lookups used by the API.
/// </summary>
public interface IMovieService
{
    Task<IReadOnlyList<Movie>> GetTrendingAsync(CancellationToken cancellationToken);

    // TODO(candidate): add a search method here (query + page) and implement it on
    // MovieService by calling ITmdbClient's new search method.
    /// <summary>
    /// Searches movies by query and page.
    /// </summary>
    Task<PagedResult<Movie>> SearchMoviesAsync(string query, int page, CancellationToken cancellationToken);

    Task<MovieDetail?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken);

    Task<IReadOnlyList<MovieVideo>> GetMovieVideosAsync(int id, CancellationToken cancellationToken);
}
