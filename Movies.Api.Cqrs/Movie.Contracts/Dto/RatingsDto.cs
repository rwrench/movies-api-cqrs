namespace Movies.Api.Contracts.Dto;

public class RatingsDto
{
    public Guid Id { get; init; }
    public Guid MovieId { get; set; }
    public float Rating { get; set; }
    public DateTime DateUpdated { get; set; }
    public Guid? UserId { get; set; }
   
}
