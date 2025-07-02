﻿using System.Text.RegularExpressions;



namespace Movies.Api.Cqrs.Application.Models
{
    public partial class Movie
    {
        public required Guid MovieId { get; init; }
        public required string Title { get; set; }
        public string Slug => GenerateSlug();

        public required int YearOfRelease { get; set; }
        public required List<string> Genres { get; set; } = new();
        public Guid? UserId { get; internal set; }

        private string GenerateSlug()
        {
            var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
                .ToLower().Replace(" ", "-");
            var sluggedYear = YearOfRelease.ToString();
            return $"{sluggedTitle}-{sluggedYear}";
        }

        [GeneratedRegex(@"[^9=0-9A-Za-z ]", RegexOptions.NonBacktracking, 5)]
        private static partial Regex SlugRegex();

    }
}
