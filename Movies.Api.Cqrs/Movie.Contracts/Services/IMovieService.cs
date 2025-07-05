

using Movies.Api.Contracts.Models;

namespace Movies.Api.Contracts.Services
{
    public interface IMovieService
    {
        Task<List<Movie>> GetMoviesAsync();
        Task<Movie?> GetMovieByIdAsync(Guid movieId);
        Task<bool> CreateMovieAsync(Movie movie);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(Guid movieId);
    }
}