namespace Movies.Api.Cqrs.Application.Dto
{
    public class CreateRatingDto
    {
        public Guid MovieId { get; set; }
        public float Rating { get; set; }
        public Guid? UserId { get; set; }
    }
}