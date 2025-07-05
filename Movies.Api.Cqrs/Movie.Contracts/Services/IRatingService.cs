using Movies.Api.Contracts.Dto;
using Movies.Api.Contracts.Models;

namespace Movies.Api.Contracts.Services
{
    public interface IRatingService
    {
        Task<List<MovieRatingWithNameDto>> GetRatingsAsync();
        Task<MovieRating?> GetRatingByIdAsync(Guid ratingId);
        Task<bool> CreateRatingAsync(CreateRatingDto rating);
        Task<bool> UpdateRatingAsync(Guid ratingId, MovieRatingWithNameDto rating);
        Task<bool> DeleteRatingAsync(Guid ratingId);
        Task<List<MovieDropdownDto>> SearchMoviesAsync(string searchTerm, int take = 10);
        Task<List<MovieDropdownDto>> GetAllMoviesForDropdownAsync();
    }
}