using System;
using System.Collections.Generic;
using System.Text;

namespace MovieSearchCase.Domain.Entities
{
    public class MovieDetail
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
        public List<string> Genres { get; set; } = new();
    }
}
