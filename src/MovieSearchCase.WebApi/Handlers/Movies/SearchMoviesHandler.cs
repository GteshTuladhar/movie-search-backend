using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearchCase.Shared;
using MovieSearchCase.Domain.Interfaces.Services;
using MovieSearchCase.WebApi.Mappers; 
namespace MovieSearchCase.WebApi.Handlers.Movies;

public class SearchMoviesHandler : IRequestHandlerAsync
{
    private readonly IMovieService _movieService;
    private readonly string _query;
    private readonly int _page;

    public SearchMoviesHandler(IMovieService movieService, string query, int page)
    {
        _movieService = movieService;
        _query = query;
        _page = page;
    }

    public async Task<IActionResult> HandleAsync(HttpRequest request)
    {
        var result = await _movieService.SearchMoviesAsync(_query, _page, request.HttpContext.RequestAborted);
        return new OkObjectResult(result);
    }
}
