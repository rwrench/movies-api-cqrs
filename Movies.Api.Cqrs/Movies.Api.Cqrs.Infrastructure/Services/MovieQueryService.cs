using FluentValidation;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Application.Services;
using Movies.Api.Cqrs.Application.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Api.Contracts.Models;
using Movies.Api.Cqrs.Infrastructure.Database;

namespace Movies.Api.Cqrs.Infrastructure.Services
{
    public class MovieQueryService : IMovieQueryService
    {
        private readonly MoviesDbContext _context;
        private readonly IValidator<GetAllMoviesOptions> _optionsValidator;
        private readonly ILogger<MovieQueryService> _logger;

        public MovieQueryService(
            MoviesDbContext context,
            IValidator<GetAllMoviesOptions> optionsValidator,
            ILogger<MovieQueryService> logger)
        {
            _context = context;
            _optionsValidator = optionsValidator;
            _logger = logger;
        }

        public async Task<IEnumerable<Movie?>> GetAllAsync(
            GetAllMoviesQuery query, 
            CancellationToken token = default)
        {
            _logger.LogInformation("Getting all movies with options");
            
            await _optionsValidator.ValidateAndThrowAsync(query.options, token);
            
            var moviesQuery = _context.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.options.Title))
            {
                moviesQuery = moviesQuery.Where(m =>
                    m.Title.StartsWith(query.options.Title));
            }

            if (query.options.YearOfRelease.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.YearOfRelease == query.options.YearOfRelease.Value);
            }

            if (query.options.UserId.HasValue)
            {
                moviesQuery = moviesQuery.Where(m => m.UserId == query.options.UserId.Value);
            }

            // Apply sorting - default to Title ascending if no sort field specified
            if (!string.IsNullOrWhiteSpace(query.options.SortField))
            {
                moviesQuery = query.options.SortField.ToLower() switch
                {
                    "title" => query.options.SortOrder == SortOrder.Descending 
                        ? moviesQuery.OrderByDescending(m => m.Title)
                        : moviesQuery.OrderBy(m => m.Title),
                    "yearofrelease" => query.options.SortOrder == SortOrder.Descending 
                        ? moviesQuery.OrderByDescending(m => m.YearOfRelease)
                        : moviesQuery.OrderBy(m => m.YearOfRelease),
                    _ => moviesQuery.OrderBy(m => m.Title)
                };
            }
            else
            {
                // Default sorting by Title ascending when no SortField is specified
                moviesQuery = moviesQuery.OrderBy(m => m.Title);
            }

            // Apply pagination
            if (query.options.Page.HasValue && query.options.PageSize.HasValue)
            {
                var skip = (query.options.Page.Value - 1) * query.options.PageSize.Value;
                moviesQuery = moviesQuery.Skip(skip).Take(query.options.PageSize.Value);
            }

            var movies = await moviesQuery.ToListAsync(token);
            
            _logger.LogInformation("Retrieved {Count} movies", movies.Count);
            return movies;
        }

        public async Task<Movie?> GetByIdAsync(GetMovieByIdQuery query, CancellationToken token = default)
        {
            _logger.LogInformation("Getting movie by ID: {MovieId}", query.Id);
            
            var movie = await _context.Movies
                .Where(m => m.MovieId == query.Id && 
                    (query.UserId == null || m.UserId == query.UserId))
                .FirstOrDefaultAsync(token);
                
            if (movie == null)
            {
                _logger.LogWarning("Movie not found with ID: {MovieId}", query.Id);
            }
            
            return movie;
        }

        public async Task<Movie?> GetBySlugAsync(GetMovieBySlugQuery query, CancellationToken token = default)
        {
            _logger.LogInformation("Getting movie by slug: {Slug}", query.Slug);
            
            var data = query.Slug.ParseTitleAndYear();
            if (data == null)
            {
                _logger.LogWarning("Could not parse slug: {Slug}", query.Slug);
                return null;
            }

            var movie = await _context.Movies
                .Where(m => m.Title == data.Value.Title && 
                    m.YearOfRelease == data.Value.YearOfRelease &&
                    (query.UserId == null || m.UserId == query.UserId))
                .FirstOrDefaultAsync(token);
                
            if (movie == null)
            {
                _logger.LogWarning("Movie not found with slug: {Slug}", query.Slug);
            }
            
            return movie;
        }
    }
}