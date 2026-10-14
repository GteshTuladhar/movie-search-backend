namespace MovieSearchCase.Domain.Models;

public class PagedResult<T>
{
    public int Page { get; init; }
    public int TotalPages { get; init; }
    public int TotalResults { get; init; }
    public IReadOnlyList<T> Results { get; init; } = Array.Empty<T>();

    public PagedResult() { }

    public PagedResult(int page, int totalPages, int totalResults, IReadOnlyList<T> results)
    {
        Page = page;
        TotalPages = totalPages;
        TotalResults = totalResults;
        Results = results;
    }
}
