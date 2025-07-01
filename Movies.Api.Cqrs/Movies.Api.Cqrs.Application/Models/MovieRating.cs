namespace Movies.Api.Cqrs.Application.Models
{
    public class MovieRating
    {
        public required Guid Id { get; init; }
        public required Guid MovieId { get; set; }
        public required float Rating { get; set; }
        public Guid? UserId { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
