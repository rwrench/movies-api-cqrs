namespace Movies.Api.Cqrs.Dto;

public class MovieImportDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int YearOfRelease { get; set; }
    public List<string> Genres { get; set; } = new();
    public List<string> Links { get; set; } = new();
}
