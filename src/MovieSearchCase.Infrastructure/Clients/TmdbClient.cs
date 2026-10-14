using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using MovieSearchCase.Domain.Entities;
using MovieSearchCase.Domain.Exceptions;
using MovieSearchCase.Domain.Interfaces.Clients;
using MovieSearchCase.Domain.Models;
using MovieSearchCase.Infrastructure.Models;

namespace MovieSearchCase.Infrastructure.Clients;

public class TmdbClient : ITmdbClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TmdbClient> _logger;

    public TmdbClient(HttpClient httpClient, ILogger<TmdbClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IReadOnlyList<Movie>> GetTrendingMoviesAsync(CancellationToken cancellationToken)
    {
        TmdbPagedResponse<TmdbMovie>? response;
        _logger.LogDebug("Executing HTTP GET to TMDB: trending/movie/week");
        try
        {
            response = await _httpClient.GetFromJsonAsync<TmdbPagedResponse<TmdbMovie>>(
                "trending/movie/week",
                cancellationToken);
        }
        catch (HttpRequestException)
        {
            throw new MovieException(
                MovieException.ExceptionTitle,
                ErrorType.UpstreamServiceUnavailable,
                "TMDB did not return trending movies.");
        }

        if (response is null)
        {
            throw new MovieException(
                MovieException.ExceptionTitle,
                ErrorType.UpstreamServiceUnavailable,
                "TMDB returned an empty trending movies response.");
        }

        return response.Results.Select(TmdbMovieMapper.ToDomainModel).ToList();
    }

    public async Task<PagedResult<Movie>> SearchMoviesAsync(string query, int page, CancellationToken cancellationToken)
    {
        TmdbPagedResponse<TmdbMovie>? response;
        _logger.LogDebug("Executing HTTP GET to TMDB: search/movie?query={Query}&page={Page}", query, page);
        try
        {
            response = await _httpClient.GetFromJsonAsync<TmdbPagedResponse<TmdbMovie>>(
                $"search/movie?query={Uri.EscapeDataString(query)}&page={page}",
                cancellationToken);
        }
        catch (HttpRequestException)
        {
            throw new MovieException(
                MovieException.ExceptionTitle,
                ErrorType.UpstreamServiceUnavailable,
                "TMDB did not return search results.");
        }

        if (response is null)
        {
            throw new MovieException(
                MovieException.ExceptionTitle,
                ErrorType.UpstreamServiceUnavailable,
                "TMDB returned an empty search response.");
        }

        var movies = response.Results.Select(TmdbMovieMapper.ToDomainModel).ToList();

        return new PagedResult<Movie>(
            response.Page,
            response.TotalPages,
            response.TotalResults,
            movies);
    }

    public async Task<MovieDetail?> GetMovieDetailsAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Executing HTTP GET to TMDB: movie/{MovieId}", id);
        var response = await _httpClient.GetAsync($"movie/{id}", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogWarning("TMDB returned 404 Not Found for movie ID {MovieId}", id);
            return null;
        }

        response.EnsureSuccessStatusCode();

        var tmdbDetail = await response.Content.ReadFromJsonAsync<TmdbMovieDetailDto>(cancellationToken: cancellationToken);
        if (tmdbDetail == null) return null;

        return new MovieDetail
        {
            Id = tmdbDetail.Id,
            Title = tmdbDetail.Title,
            Overview = tmdbDetail.Overview,
            PosterPath = tmdbDetail.PosterPath,
            BackdropPath = tmdbDetail.BackdropPath,
            VoteAverage = tmdbDetail.VoteAverage,
            ReleaseDate = tmdbDetail.ReleaseDate,
            Runtime = tmdbDetail.Runtime,
            Tagline = tmdbDetail.Tagline,
            Genres = tmdbDetail.Genres?.Select(g => g.Name).ToList() ?? new List<string>()
        };
    }

    public async Task<IReadOnlyList<MovieVideo>> GetMovieVideosAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Executing HTTP GET to TMDB: movie/{MovieId}/videos", id);
        var response = await _httpClient.GetAsync($"movie/{id}/videos", cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogWarning("TMDB returned 404 Not Found for movie ID {MovieId}", id);
            return Array.Empty<MovieVideo>();
        }

        response.EnsureSuccessStatusCode();

        var videoResponse = await response.Content.ReadFromJsonAsync<TmdbVideoResponse>(cancellationToken: cancellationToken);
        if (videoResponse?.Results == null) return Array.Empty<MovieVideo>();

        return videoResponse.Results
            .Where(v => string.Equals(v.Site, "YouTube", StringComparison.OrdinalIgnoreCase))
            .Select(v => new MovieVideo
            {
                Key = v.Key,
                Name = v.Name,
                Site = v.Site,
                Type = v.Type
            })
            .ToList();
    }
}
