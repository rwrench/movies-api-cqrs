using Movies.Api.Contracts.Dto;
using Movies.Api.Contracts.Models;
using System.Net.Http.Json;

namespace Movies.Api.Contracts.Services
{
    public class RatingService : IRatingService
    {
        private readonly HttpClient _httpClient;
        private readonly IApiService _apiService;

        public RatingService(HttpClient httpClient, IApiService apiService)
        {
            _httpClient = httpClient;
            _apiService = apiService;
        }

        public async Task<List<MovieRatingWithNameDto>> GetRatingsAsync()
        {
            try
            {
                var ratings = await _httpClient.GetFromJsonAsync<List<MovieRatingWithNameDto>>(_apiService.GetRatingsEndpoint());
                return ratings ?? new List<MovieRatingWithNameDto>();
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed to load ratings");
            }
        }

        public async Task<MovieRating?> GetRatingByIdAsync(Guid ratingId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<MovieRating>(_apiService.GetRatingByIdEndpoint(ratingId));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> CreateRatingAsync(CreateRatingDto rating)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiService.GetRatingsEndpoint(), rating);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateRatingAsync(Guid ratingId, MovieRatingWithNameDto rating)
        {
            try
            {
                var updateDto = new
                {
                    MovieId = rating.MovieId,
                    Rating = rating.Rating,
                    UserId = rating.UserId,
                    DateUpdated = DateTime.UtcNow
                };

                var response = await _httpClient.PutAsJsonAsync(_apiService.GetRatingByIdEndpoint(ratingId), updateDto);
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRatingAsync(Guid ratingId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_apiService.GetRatingByIdEndpoint(ratingId));
                return response.IsSuccessStatusCode;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<MovieDropdownDto>> SearchMoviesAsync(string searchTerm, int take = 10)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 2)
                {
                    return new List<MovieDropdownDto>();
                }

                var searchUrl = _apiService.GetMovieSearchEndpoint(searchTerm, take);
                var results = await _httpClient.GetFromJsonAsync<List<MovieDropdownDto>>(searchUrl);
                return results ?? new List<MovieDropdownDto>();
            }
            catch (Exception)
            {
                return new List<MovieDropdownDto>();
            }
        }

        public async Task<List<MovieDropdownDto>> GetAllMoviesForDropdownAsync()
        {
            try
            {
                // Get all movies and convert to dropdown format
                var movies = await _httpClient.GetFromJsonAsync<List<Movie>>(_apiService.GetMoviesEndpoint());
                if (movies == null) return new List<MovieDropdownDto>();

                return movies.Select(m => new MovieDropdownDto
                {
                    Id = m.MovieId,
                    Title = m.Title,
                    YearOfRelease = m.YearOfRelease
                }).ToList();
            }
            catch (Exception)
            {
                return new List<MovieDropdownDto>();
            }
        }
    }
}