using MovieSearchCase.Shared;
using MovieSearchCase.Domain.Models;
using MovieSearchCase.WebApi.Models.Movies;

namespace MovieSearchCase.WebApi.Handlers.Movies;

public interface IMoviesRequestHandlerFactory
{
    IRequestHandlerAsync GetTrendingMovies();

    // TODO(candidate): add a SearchMovies(string query, int page) method here once you've
    // added the search handler, then wire it up in MoviesRequestHandlerFactory below.
    IRequestHandlerAsync SearchMovies(string query, int page);

    IRequestHandlerAsync GetMovieDetails(int id);

    IRequestHandlerAsync GetMovieVideos(int id);
}
