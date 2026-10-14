using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieSearchCase.Domain.Entities;
using MovieSearchCase.Domain.Interfaces.Services;
using MovieSearchCase.Domain.Models;
using MovieSearchCase.WebApi.Handlers.Movies;
using Xunit;

public class SearchMoviesHandlerTests
{
    private readonly Mock<IMovieService> _movieServiceMock;
    private readonly SearchMoviesHandler _handler;

    public SearchMoviesHandlerTests()
    {
        _movieServiceMock = new Mock<IMovieService>();
        _handler = new SearchMoviesHandler(_movieServiceMock.Object, "Matrix", 1);
    }

    [Fact]
    public async Task HandleAsync_ReturnsOkWithPagedResult()
    {
        // Arrange
        var pagedResult = new PagedResult<Movie>
        {
            Page = 1,
            TotalPages = 1,
            TotalResults = 1,
            Results = new List<Movie> { new Movie { Id = 603, Title = "The Matrix" } }
        };

        _movieServiceMock
            .Setup(s => s.SearchMoviesAsync("Matrix", 1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        var httpContext = new DefaultHttpContext();
        var request = httpContext.Request;

        // Act
        var result = await _handler.HandleAsync(request);

        // Assert
        var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
        var value = okResult.Value.Should().BeOfType<PagedResult<Movie>>().Subject;
        value.TotalResults.Should().Be(1);
        value.Results.Should().ContainSingle(m => m.Title == "The Matrix");
    }
}
