namespace Movies.Api.Cqrs.Application.Dto
{
    public class MovieDto
    {
        public Guid Id { get; set; } 
        public string Title { get; set; } = string.Empty;
        public int YearOfRelease { get; set; }
        public float Rating { get; set; } = 0;
        public int? UserRating { get; set; } = null;
        public List<string> Genres { get; set; } = new List<string>();
        public Guid? UserId { get; set; } = null;
      
      
    }
}
