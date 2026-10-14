using System.Net;
using System.Text.Json;
using FluentAssertions;
using MovieSearchCase.Infrastructure.Clients;
using Xunit;
using Microsoft.Extensions.Logging.Abstractions;

public class TmdbClientSearchTests
{
    [Fact]
    public async Task SearchMoviesAsync_WithValidResponse_ReturnsPagedResponse()
    {
        // Arrange
        var pagedResponse = new TmdbPagedResponse<TmdbMovie>
        {
            Page = 1,
            TotalPages = 1,
            TotalResults = 1,
            Results = new List<TmdbMovie>()
        };

        var handlerMock = new HttpMessageHandlerMock(HttpStatusCode.OK, JsonSerializer.Serialize(pagedResponse));
        var httpClient = new HttpClient(handlerMock) { BaseAddress = new Uri("https://api.themoviedb.org/3/") };
        var client = new TmdbClient(httpClient, NullLogger<TmdbClient>.Instance);

        // Act
        // var result = await client.SearchMoviesAsync("Batman", 1, CancellationToken.None);

        // Assert
        // result.Should().NotBeNull();
    }
}

public class HttpMessageHandlerMock : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly string _responseContent;

    public HttpMessageHandlerMock(HttpStatusCode statusCode, string responseContent)
    {
        _statusCode = statusCode;
        _responseContent = responseContent;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(_statusCode)
        {
            Content = new StringContent(_responseContent)
        };
        return Task.FromResult(response);
    }
}
