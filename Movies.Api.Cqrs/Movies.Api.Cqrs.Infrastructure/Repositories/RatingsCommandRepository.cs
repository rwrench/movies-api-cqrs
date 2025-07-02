using Microsoft.EntityFrameworkCore;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Infrastructure.Repositories;

public class RatingsCommandRepository : IRatingsCommandRepository
{
    MoviesDbContext _context;

    public RatingsCommandRepository(MoviesDbContext context)
    {
        _context = context;
    }


    public async Task<bool> RateMovieAsync(
        Guid movieId,
        float rating, 
        Guid userId, 
        CancellationToken token)
    {
        var existing = await _context.Ratings
            .FirstOrDefaultAsync(r => r.MovieId == movieId
            && r.UserId == userId, token);

        if (existing != null)
        {
            existing.Rating = rating;
            existing.DateUpdated = DateTime.UtcNow;
        }
        else
        {
            _context.Ratings.Add(new MovieRating
            {
                Id = Guid.NewGuid(),
                MovieId = movieId,
                Rating = rating,
                UserId = userId,
                DateUpdated = DateTime.UtcNow
            });
        }

        return await _context.SaveChangesAsync(token) > 0;
    }
}
