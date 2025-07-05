using Movies.Api.Cqrs.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Api.Contracts.Dto;
using AutoMapper;
using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Infrastructure.Services;

public class RatingsQueryService : IRatingsQueryService
{
    private readonly MoviesDbContext _context;
    private readonly ILogger<RatingsQueryService> _logger;
    private readonly IMapper _mapper;

    public RatingsQueryService(
        MoviesDbContext context,
        ILogger<RatingsQueryService> logger,
        IMapper mapper)
    {
        _context = context;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MovieRatingWithNameDto>> GetAllAsync(CancellationToken token = default)
    {
        _logger.LogInformation("Getting all movie ratings with movie names");
        
        var ratingsWithMovies = await _context.Ratings
            .Join(_context.Movies,
                rating => rating.MovieId,
                movie => movie.MovieId,
                (rating, movie) => new { Rating = rating, Movie = movie })
            .ToListAsync(token);
        
        var result = ratingsWithMovies.Select(x => 
            _mapper.Map<MovieRatingWithNameDto>((x.Rating, x.Movie)));
            
        _logger.LogInformation("Retrieved {Count} movie ratings", result.Count());
        return result;
    }
}