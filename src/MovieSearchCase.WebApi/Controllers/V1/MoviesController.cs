using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using MovieSearchCase.Domain.Models;
using MovieSearchCase.Domain.Entities;
using MovieSearchCase.WebApi.Models.Movies;
using MovieSearchCase.WebApi.Application.Versioning;
using MovieSearchCase.WebApi.Handlers.Movies;

namespace MovieSearchCase.WebApi.Controllers.V1;

[ApiVersion("1.0")]
[VersionedRoute]
public class MoviesController : ApiControllerBase
{
    private readonly IMoviesRequestHandlerFactory _requestHandlerFactory;
    private readonly ILogger<MoviesController> _logger;

    public MoviesController(
        IMoviesRequestHandlerFactory requestHandlerFactory,
        ILogger<MoviesController> logger)
    {
        _requestHandlerFactory = requestHandlerFactory;
        _logger = logger;
    }

    /// <summary>
    /// Returns this week's TMDB-backed trending movies.
    /// </summary>
    [HttpGet("trending")]
    [SwaggerOperation(Summary = "Get trending movies", OperationId = "GetTrendingMovies")]
    [ProducesResponseType(typeof(IEnumerable<MovieSearchCase.WebApi.Models.Movies.Movie>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetTrending()
    {
        return await _requestHandlerFactory.GetTrendingMovies().HandleAsync(Request);
    }


    [HttpGet("search")]
    [ProducesResponseType(typeof(PagedResult<MovieSearchCase.WebApi.Models.Movies.Movie>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Search(
        [FromQuery] string query,
        [FromQuery] int? page,
        CancellationToken cancellationToken)
    {
        int currentPage = page ?? 1;

        if (string.IsNullOrWhiteSpace(query))
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The 'query' parameter is required."
            });
        }

        if (currentPage < 1 || currentPage > 500)
        {
            return BadRequest(new ProblemDetails
            {
                Title = "Invalid request",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The 'page' parameter must be a whole number from 1 to 500."
            });
        }

        return await _requestHandlerFactory.SearchMovies(query, currentPage).HandleAsync(Request);
    }


    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MovieDetail), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        return await _requestHandlerFactory.GetMovieDetails(id).HandleAsync(Request);
    }


    [HttpGet("{id:int}/videos")]
    [ProducesResponseType(typeof(IEnumerable<MovieVideo>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> GetVideos(int id, CancellationToken cancellationToken)
    {
        return await _requestHandlerFactory.GetMovieVideos(id).HandleAsync(Request);
    }
}
