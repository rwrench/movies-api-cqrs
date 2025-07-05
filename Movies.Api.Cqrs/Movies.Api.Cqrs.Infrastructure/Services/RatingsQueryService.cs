
using Movies.Api.Cqrs.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Api.Contracts.Dto;

namespace Movies.Api.Cqrs.Infrastructure.Services;

public class RatingsQueryService : IRatingsQueryService
{
    private readonly MoviesDbContext _context;
    private readonly ILogger<RatingsQueryService> _logger;

    public RatingsQueryService(
        MoviesDbContext context,
        ILogger<RatingsQueryService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<MovieRatingWithNameDto>> GetAllAsync(CancellationToken token = default)
    {
        _logger.LogInformation("Getting all movie ratings with movie names");
        
        var ratingsWithNames = await _context.Ratings
            .Join(_context.Movies,
                rating => rating.MovieId,
                movie => movie.MovieId,
                (rating, movie) => new MovieRatingWithNameDto
                {
                    Id = rating.Id,
                    MovieId = rating.MovieId,
                    Rating = rating.Rating,
                    UserId = rating.UserId,
                    DateUpdated = rating.DateUpdated,
                    MovieName = movie.Title
                })
            .ToListAsync(token);
            
        _logger.LogInformation("Retrieved {Count} movie ratings", ratingsWithNames.Count);
        return ratingsWithNames;
    }
}