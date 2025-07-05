namespace Movies.Api.Contracts.Dto
{
    public class MovieDropdownDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int YearOfRelease { get; set; }
        public string DisplayText => $"{Title} ({YearOfRelease})";
    }
}