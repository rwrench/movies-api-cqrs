using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<MovieRating?>> GetAllAsync(CancellationToken token = default)
        {
           return await _context.Ratings
                .AsNoTracking()
                .ToListAsync(token);    
        }
    }
}
