using Microsoft.EntityFrameworkCore;
using Movies.Api.Cqrs.Application.Extensions;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace Movies.Api.Cqrs.Infrastructure.Repositories
{
    public class MovieCommandRepository : IMovieCommandRepository
    {
        readonly MoviesDbContext _context;
        readonly ILogger<MovieCommandRepository> _logger;

        public MovieCommandRepository(MoviesDbContext context, ILogger<MovieCommandRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(
            Movie movie, 
            CancellationToken token)
        {
            _logger.LogInformation("Creating movie with ID: {MovieId}, Title: {Title}", movie.MovieId, movie.Title);
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync(token);
            _logger.LogInformation("Successfully created movie with ID: {MovieId}", movie.MovieId);
            return movie.MovieId;
        }

        public async Task<bool> DeleteAsync(
            Guid id, 
            Guid? userId = null,
            CancellationToken token = default)
        {
            _logger.LogInformation("Attempting to delete movie with ID: {Id}, UserId: {UserId}", id, userId);
            
            var movie = await _context.Movies.Where(m => m.MovieId == id &&
                (userId == null || m.UserId == userId))
                .FirstOrDefaultAsync(token);

            if (movie is null)
            {
                _logger.LogWarning("Movie not found for deletion with ID: {Id}", id);
                return false;
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync(token);
            _logger.LogInformation("Successfully deleted movie with ID: {Id}", id);
            return true;
        }

        public async Task<bool> UpdateByIdAsync(Guid id, string title, int yearOfRelease, List<string> genres, CancellationToken token)
        {
            _logger.LogInformation("UpdateByIdAsync called with ID: {Id}, Title: {Title}, Year: {Year}", 
                id, title, yearOfRelease);
            
            Console.WriteLine($"REPOSITORY UPDATE BY ID - Searching for movie ID: {id}");
            
            // Find the existing movie in the database
            var existingMovie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id, token);

            if (existingMovie == null)
            {
                _logger.LogWarning("Movie not found for update with ID: {MovieId}", id);
                Console.WriteLine($"REPOSITORY UPDATE BY ID - Movie NOT FOUND with ID: {id}");
                return false;
            }

            _logger.LogInformation("Found existing movie: {Title}, updating properties", existingMovie.Title);
            Console.WriteLine($"REPOSITORY UPDATE BY ID - Found movie: {existingMovie.Title}");

            // Update the properties of the tracked entity
            var oldTitle = existingMovie.Title;
            var oldYear = existingMovie.YearOfRelease;
            
            existingMovie.Title = title;
            existingMovie.YearOfRelease = yearOfRelease;
            existingMovie.Genres = genres;

            _logger.LogInformation("Updated movie properties - Title: {OldTitle} -> {NewTitle}, Year: {OldYear} -> {NewYear}", 
                oldTitle, title, oldYear, yearOfRelease);

            // Save changes
            Console.WriteLine($"REPOSITORY UPDATE BY ID - Saving changes...");
            var affectedRows = await _context.SaveChangesAsync(token);
            
            _logger.LogInformation("SaveChanges completed. Affected rows: {AffectedRows}", affectedRows);
            Console.WriteLine($"REPOSITORY UPDATE BY ID - Affected rows: {affectedRows}");
            
            var success = affectedRows > 0;
            _logger.LogInformation("Update operation result: {Success}", success);
            
            return success;
        }
    }
}
