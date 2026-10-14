using MovieSearchCase.Domain.Entities;
using MovieSearchCase.Domain.Interfaces.Clients;
using MovieSearchCase.Domain.Interfaces.Services;
using MovieSearchCase.Domain.Models;
using Microsoft.Extensions.Logging;

namespace MovieSearchCase.Infrastructure.Services;

public class MovieService : IMovieService
{
    private readonly ITmdbClient _tmdbClient;
    private readonly ILogger<MovieService> _logger;

    public MovieService(ITmdbClient tmdbClient, ILogger<MovieService> logger)
    {
        _tmdbClient = tmdbClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Movie>> GetTrendingAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("MovieService fetching trending movies.");
        var result = await _tmdbClient.GetTrendingMoviesAsync(cancellationToken);
        _logger.LogInformation("MovieService successfully retrieved trending movies, count: {Count}", result.Count);
        return result;
    }

    public async Task<PagedResult<Movie>> SearchMoviesAsync(string query, int page, CancellationToken cancellationToken)
    {
        _logger.LogInformation("MovieService searching movies with query '{Query}' on page {Page}", query, page);
        var result = await _tmdbClient.SearchMoviesAsync(query, page, cancellationToken);
        _logger.LogInformation("MovieService search completed for query '{Query}'", query);
        return result;
    }

    public async Task<MovieDetail?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("MovieService fetching details for movie ID {MovieId}", id);
        var result = await _tmdbClient.GetMovieDetailsAsync(id, cancellationToken);

        if (result == null)
        {
            _logger.LogWarning("MovieService found no details for movie ID {MovieId}", id);
        }

        return result;
    }

    public async Task<IReadOnlyList<MovieVideo>> GetMovieVideosAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("MovieService fetching videos for movie ID {MovieId}", id);
        var result = await _tmdbClient.GetMovieVideosAsync(id, cancellationToken);
        _logger.LogInformation("MovieService retrieved {VideoCount} videos for movie ID {MovieId}", result.Count, id);
        return result;
    }
}
