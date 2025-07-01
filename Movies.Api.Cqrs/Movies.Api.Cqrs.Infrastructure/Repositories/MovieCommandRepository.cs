using Microsoft.EntityFrameworkCore;
using Movies.Api.Cqrs.Application.Extensions;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Infrastructure.Repositories
{
    public class MovieCommandRepository : IMovieCommandRepository
    {
        readonly MoviesDbContext _context;

        public MovieCommandRepository(MoviesDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(
            Movie movie, 
            CancellationToken token)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            return movie.Id;
        }

        public async Task<bool> DeleteAsync(
            Guid id, 
            Guid? userId = null,
            CancellationToken token = default)
        {
            var movie = await _context.Movies.Where(m => m.Id == id &&
                (userId == null || m.UserId == userId))
                .FirstOrDefaultAsync();

            if (movie is null)
            {
                return false;
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<Movie?> GetByIdAsync(
            Guid id, 
            Guid? userId = null,
            CancellationToken token = default)
        {
            return _context.Movies.Where(m => m.Id == id &&
                (userId == null || m.UserId == userId))
                .FirstOrDefaultAsync();
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

        public Task<bool> UpdateAsync(Movie movie, CancellationToken token)
        {
           _context.Movies.Update(movie);
            return _context.SaveChangesAsync()
                .ContinueWith(t => t.Result > 0); 
        }
    }
}
