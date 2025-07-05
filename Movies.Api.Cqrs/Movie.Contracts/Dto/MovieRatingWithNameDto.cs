

namespace Movies.Api.Contracts.Dto;

public class MovieRatingWithNameDto
{
    public Guid Id { get; set; }
    public Guid MovieId { get; set; }
    public float Rating { get; set; }
    public Guid? UserId { get; set; }
    public DateTime DateUpdated { get; set; }
    public string MovieName { get; set; } = string.Empty;
}
