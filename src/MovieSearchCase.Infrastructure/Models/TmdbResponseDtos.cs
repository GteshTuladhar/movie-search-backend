using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchCase.Infrastructure.Models;

internal class TmdbMovieDetailDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? BackdropPath { get; set; }
    public double VoteAverage { get; set; }
    public string? ReleaseDate { get; set; }
    public int? Runtime { get; set; }
    public string? Tagline { get; set; }
    public List<TmdbGenreDto>? Genres { get; set; }
}

internal class TmdbGenreDto
{
    public string Name { get; set; } = string.Empty;
}

internal class TmdbVideoResponse
{
    public List<TmdbVideoDto>? Results { get; set; }
}

internal class TmdbVideoDto
{
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Site { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
