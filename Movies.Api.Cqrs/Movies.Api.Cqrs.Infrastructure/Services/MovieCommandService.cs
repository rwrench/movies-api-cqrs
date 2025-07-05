using AutoMapper;
using FluentValidation;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Api.Contracts.Models;
using Movies.Api.Cqrs.Infrastructure.Database;

namespace Movies.Api.Cqrs.Infrastructure.Services
{
    public class MovieCommandService : IMovieCommandService
    {
        private readonly MoviesDbContext _context;
        private readonly IValidator<Movie> _movieValidator;
        private readonly IValidator<UpdateMovieCommand> _updateValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<MovieCommandService> _logger;

        public MovieCommandService(
            MoviesDbContext context,
            IValidator<Movie> movieValidator,
            IValidator<UpdateMovieCommand> updateValidator,
            IMapper mapper,
            ILogger<MovieCommandService> logger)
        {
            _context = context;
            _movieValidator = movieValidator;
            _updateValidator = updateValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Guid> CreateAsync(
            CreateMovieCommand command,
            CancellationToken token = default)
        {
            _logger.LogInformation("Creating movie with title: {Title}", command.Title);
            
            var movie = _mapper.Map<Movie>(command);
            await _movieValidator.ValidateAndThrowAsync(movie, token);
            
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync(token);
            
            _logger.LogInformation("Successfully created movie with ID: {MovieId}", movie.MovieId);
            return movie.MovieId;
        }

        public async Task<bool> DeleteAsync(DeleteMovieCommand command, 
            CancellationToken token = default)
        {
            _logger.LogInformation("Attempting to delete movie with ID: {MovieId}, UserId: {UserId}", 
                command.MovieId, command.UserId);
            
            var movie = await _context.Movies
                .Where(m => m.MovieId == command.MovieId &&
                    (command.UserId == null || m.UserId == command.UserId))
                .FirstOrDefaultAsync(token);

            if (movie == null)
            {
                _logger.LogWarning("Movie not found for deletion with ID: {MovieId}", command.MovieId);
                return false;
            }

            _context.Movies.Remove(movie);
            var affectedRows = await _context.SaveChangesAsync(token);
            
            _logger.LogInformation("Successfully deleted movie with ID: {MovieId}", command.MovieId);
            return affectedRows > 0;
        }

        public async Task<bool> UpdateAsync(UpdateMovieCommand command, 
            CancellationToken token = default)
        {
            _logger.LogInformation("Updating movie with ID: {MovieId}, Title: {Title}", 
                command.MovieId, command.Title);
            
            // Validate the update command
            await _updateValidator.ValidateAndThrowAsync(command, token);
            
            // Find the existing movie in the database
            var existingMovie = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == command.MovieId, token);

            if (existingMovie == null)
            {
                _logger.LogWarning("Movie not found for update with ID: {MovieId}", command.MovieId);
                return false;
            }

            // Update the properties of the tracked entity
            existingMovie.Title = command.Title;
            existingMovie.YearOfRelease = command.YearOfRelease;
            existingMovie.Genres = command.Genres;

            var affectedRows = await _context.SaveChangesAsync(token);
            
            _logger.LogInformation("Successfully updated movie with ID: {MovieId}, affected rows: {AffectedRows}", 
                command.MovieId, affectedRows);
            
            return affectedRows > 0;
        }
    }
}