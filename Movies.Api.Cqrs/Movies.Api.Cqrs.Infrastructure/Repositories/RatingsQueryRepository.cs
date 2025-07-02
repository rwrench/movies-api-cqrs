using Microsoft.EntityFrameworkCore;
using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Infrastructure.Repositories
{
    public class RatingsQueryRepository : IRatingsQueryRepository
    {
        readonly MoviesDbContext _context;

        public RatingsQueryRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MovieRatingWithNameDto>> GetAllAsync(CancellationToken token = default)
        {
            return await (
                from rating in _context.Ratings.AsNoTracking()
                join movie in _context.Movies.AsNoTracking()
                    on rating.MovieId equals movie.MovieId
                select new MovieRatingWithNameDto
                {
                    Id = rating.Id,
                    MovieId = rating.MovieId,
                    Rating = rating.Rating,
                    UserId = rating.UserId,
                    DateUpdated = rating.DateUpdated,
                    MovieName = movie.Title
                }
            ).ToListAsync(token);
        }
    }
}
