using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearchCase.Shared;
using MovieSearchCase.Domain.Interfaces.Services;

namespace MovieSearchCase.WebApi.Handlers.Movies
{
    public class GetMovieVideosHandler : IRequestHandlerAsync
    {
        private readonly IMovieService _movieService;
        private readonly int _id;

        public GetMovieVideosHandler(IMovieService movieService, int id)
        {
            _movieService = movieService;
            _id = id;
        }

        public async Task<IActionResult> HandleAsync(HttpRequest request)
        {
            var videos = await _movieService.GetMovieVideosAsync(_id, request.HttpContext.RequestAborted);
            var youtubeVideos = videos.Where(v => v.Site.Equals("YouTube", StringComparison.OrdinalIgnoreCase)).ToList();
            return new OkObjectResult(youtubeVideos);
        }
    }
}
