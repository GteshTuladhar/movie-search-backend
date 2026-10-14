using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearchCase.Shared;
using MovieSearchCase.Domain.Interfaces.Services;

namespace MovieSearchCase.WebApi.Handlers.Movies;

public class GetMovieDetailsHandler : IRequestHandlerAsync
{
    private readonly IMovieService _movieService;
    private readonly int _id;

    public GetMovieDetailsHandler(IMovieService movieService, int id)
    {
        _movieService = movieService;
        _id = id;
    }

    public async Task<IActionResult> HandleAsync(HttpRequest request)
    {
        var movie = await _movieService.GetMovieDetailsAsync(_id, request.HttpContext.RequestAborted);
        if (movie == null)
        {
            return new NotFoundResult();
        }

        return new OkObjectResult(movie);
    }
}
