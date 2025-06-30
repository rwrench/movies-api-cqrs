namespace Movies.Api.Cqrs.Application.Models
{
    public class MovieRating
    {
        public required Guid MovieId { get; set; }
        public required string Slug { get; set; }

        public required string Rating { get; set; }
    }
}
