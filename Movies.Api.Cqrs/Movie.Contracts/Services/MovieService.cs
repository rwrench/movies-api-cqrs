using Movies.Api.Contracts.Models;
using System.Net.Http.Json;

namespace Movies.Api.Contracts.Services
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;
        private readonly IApiService _apiService;

        public MovieService(HttpClient httpClient, IApiService apiService)
        {
            _httpClient = httpClient;
            _apiService = apiService;
        }

        public async Task<List<Movie>> GetMoviesAsync()
        {
            try
            {
                var movies = await _httpClient.GetFromJsonAsync<List<Movie>>(_apiService.GetMoviesEndpoint());
                return movies ?? new List<Movie>();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed to load movies");
            }
        }

        public async Task<Movie?> GetMovieByIdAsync(Guid movieId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Movie>(_apiService.GetMovieByIdEndpoint(movieId));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CreateMovieAsync(Movie movie)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiService.GetMoviesEndpoint(), movie);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(_apiService.GetMovieByIdEndpoint(movie.MovieId), movie);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteMovieAsync(Guid movieId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_apiService.GetMovieByIdEndpoint(movieId));
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}