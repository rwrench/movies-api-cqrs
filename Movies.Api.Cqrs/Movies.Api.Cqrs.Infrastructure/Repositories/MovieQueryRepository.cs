using Microsoft.EntityFrameworkCore;
using Movies.Api.Cqrs.Application.Extensions;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Infrastructure.Repositories
{
    public class MovieQueryRepository : IMovieQueryRepository
    {
        readonly MoviesDbContext _context;

        public MovieQueryRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie?>> GetAllAsync(
            GetAllMoviesOptions options,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(options.Title))
                query = query.Where(m => m.Title.Contains(options.Title));

            if (options.YearOfRelease.HasValue)
                query = query.Where(m => m.YearOfRelease == 
                    options.YearOfRelease.Value);

            // Sorting
            if (!string.IsNullOrEmpty(options.SortField))
            {
                if (options.SortField.Equals("title", StringComparison.OrdinalIgnoreCase))
                    query = options.SortOrder == SortOrder.Descending
                        ? query.OrderByDescending(m => m.Title)
                        : query.OrderBy(m => m.Title);
                else if (options.SortField.Equals("yearofrelease", StringComparison.OrdinalIgnoreCase))
                    query = options.SortOrder == SortOrder.Descending
                        ? query.OrderByDescending(m => m.YearOfRelease)
                        : query.OrderBy(m => m.YearOfRelease);
            }
            else
            {
                query = query.OrderBy(m => m.Title);
            }

            // Paging
            if (options.Page.HasValue && options.PageSize.HasValue)
                query = query.Skip((options.Page.Value - 1) * options.PageSize.Value).Take(options.PageSize.Value);

            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Movie?> GetByIdAsync(
            Guid id, Guid? userId = null, 
            CancellationToken cancellationToken = default)
        {
            return await _context.Movies
                .Where(m => m.Id == id && (userId == null || m.UserId == userId))
                .FirstOrDefaultAsync(cancellationToken);

        }

        public Task<Movie?> GetBySlugAsync(
         string slug,
         Guid? userId = null,
         CancellationToken token = default)
        {
            var data = slug.ParseTitleAndYear();
            if (data == null)
                return Task.FromResult<Movie?>(null);

            var query = _context.Movies.Where(m =>
                m.Title == data.Value.Title &&
                m.YearOfRelease == data.Value.YearOfRelease &&
                (userId == null || m.UserId == userId)
            );

            return query.FirstOrDefaultAsync(token);
        }

    }
}
