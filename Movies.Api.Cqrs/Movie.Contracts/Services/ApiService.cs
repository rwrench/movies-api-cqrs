using Movies.Api.Contracts.Models;

namespace Movies.Api.Contracts.Services
{
    public class ApiService : IApiService
    {
        private readonly ApiSettings _apiSettings;

        public ApiService(ApiSettings apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public string GetMoviesEndpoint()
        {
            return $"{_apiSettings.BaseUrl}/{_apiSettings.Endpoints.Movies}";
        }

        public string GetRatingsEndpoint()
        {
            return $"{_apiSettings.BaseUrl}/{_apiSettings.Endpoints.Ratings}";
        }

        public string GetMovieSearchEndpoint(string searchTerm, int take = 10)
        {
            return $"{GetMoviesEndpoint()}/search?searchTerm={Uri.EscapeDataString(searchTerm)}&take={take}";
        }

        public string GetMovieByIdEndpoint(Guid movieId)
        {
            return $"{GetMoviesEndpoint()}/{movieId}";
        }

        public string GetRatingByIdEndpoint(Guid ratingId)
        {
            return $"{GetRatingsEndpoint()}/{ratingId}";
        }
    }
}