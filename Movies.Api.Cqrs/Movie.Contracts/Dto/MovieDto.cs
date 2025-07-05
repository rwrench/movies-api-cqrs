namespace Movies.Api.Contracts.Dto
{
    public class MovieDto
    {
        public Guid MovieId { get; set; } 
        public string Title { get; set; } = string.Empty;
        public int YearOfRelease { get; set; }
        public List<string> Genres { get; set; } = new List<string>();
        public Guid? UserId { get; set; } = null;
      
      
    }
}
