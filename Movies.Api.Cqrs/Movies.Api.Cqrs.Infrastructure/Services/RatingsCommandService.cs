using FluentValidation;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Api.Contracts.Models;
using Movies.Api.Cqrs.Infrastructure.Database;

namespace Movies.Api.Cqrs.Infrastructure.Services
{
    public class RatingsCommandService : IRatingsCommandService
    {
        private readonly MoviesDbContext _context;
        private readonly IValidator<RateMovieCommand> _validator;
        private readonly ILogger<RatingsCommandService> _logger;

        public RatingsCommandService(
            MoviesDbContext context,
            IValidator<RateMovieCommand> validator,
            ILogger<RatingsCommandService> logger)
        {
            _context = context;
            _validator = validator;
            _logger = logger;
        }

        public async Task<bool> RateMovieAsync(
            RateMovieCommand command, 
            CancellationToken token = default)
        {
            _logger.LogInformation("Rating movie {MovieId} with rating {Rating} by user {UserId}", 
                command.MovieId, command.Rating, command.UserId);
            
            await _validator.ValidateAndThrowAsync(command, token);

            // Check if rating already exists for this user and movie
            var existingRating = await _context.Ratings
                .FirstOrDefaultAsync(r => r.MovieId == command.MovieId && r.UserId == command.UserId, token);

            if (existingRating != null)
            {
                // Update existing rating
                existingRating.Rating = command.Rating;
                existingRating.DateUpdated = DateTime.UtcNow;
                _logger.LogInformation("Updated existing rating for movie {MovieId} by user {UserId}", 
                    command.MovieId, command.UserId);
            }
            else
            {
                // Create new rating
                var newRating = new MovieRating
                {
                    Id = Guid.NewGuid(),
                    MovieId = command.MovieId,
                    Rating = command.Rating,
                    UserId = command.UserId,
                    DateUpdated = DateTime.UtcNow
                };
                
                _context.Ratings.Add(newRating);
                _logger.LogInformation("Created new rating for movie {MovieId} by user {UserId}", 
                    command.MovieId, command.UserId);
            }

            var affectedRows = await _context.SaveChangesAsync(token);
            return affectedRows > 0;
        }
    }
}