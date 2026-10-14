using MovieSearchCase.Domain.Interfaces.Services;
using MovieSearchCase.Domain.Interfaces.Clients;
using MovieSearchCase.Shared;
using MovieSearchCase.Domain.Models;
using MovieSearchCase.WebApi.Models.Movies;


namespace MovieSearchCase.WebApi.Handlers.Movies;

public class MoviesRequestHandlerFactory : IMoviesRequestHandlerFactory
{
    private readonly IMovieService _movieService;

    public MoviesRequestHandlerFactory(IMovieService movieService)
    {
        _movieService = movieService;
    }

    public IRequestHandlerAsync GetTrendingMovies() => new GetTrendingMoviesHandler(_movieService);

    public IRequestHandlerAsync SearchMovies(string query, int page)
    {
        return new SearchMoviesHandler(_movieService, query, page);
    }

    public IRequestHandlerAsync GetMovieDetails(int id)
    {
        return new GetMovieDetailsHandler(_movieService, id);
    }

    public IRequestHandlerAsync GetMovieVideos(int id)
    {
        return new GetMovieVideosHandler(_movieService, id);
    }
}
