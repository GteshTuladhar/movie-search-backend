using MovieSearchCase.Domain.Entities;
using MovieSearchCase.Domain.Models;

namespace MovieSearchCase.Domain.Interfaces.Clients;

/// <summary>
/// Talks to The Movie Database (TMDB) REST API. See https://developer.themoviedb.org/reference.
/// </summary>
public interface ITmdbClient
{
    /// <summary>
    /// Returns this week's trending movies (TMDB's /trending/movie/week endpoint).
    /// </summary>
    Task<IReadOnlyList<Movie>> GetTrendingMoviesAsync(CancellationToken cancellationToken);

    // TODO(candidate): add a method here for TMDB's /search/movie endpoint (query + page)
    // and implement it on TmdbClient, following the same pattern as GetTrendingMoviesAsync.
    Task<PagedResult<Movie>> SearchMoviesAsync(string query, int page, CancellationToken cancellationToken);

    Task<MovieDetail?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<MovieVideo>> GetMovieVideosAsync(int id, CancellationToken cancellationToken);
}
