namespace Movies.Api.Contracts.Models
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public ApiEndpoints Endpoints { get; set; } = new();
    }

    public class ApiEndpoints
    {
        public string Movies { get; set; } = string.Empty;
        public string Ratings { get; set; } = string.Empty;
    }
}