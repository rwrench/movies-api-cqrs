using Microsoft.EntityFrameworkCore;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            CancellationToken cancellationToken = default)
        {
            return await _context.Movies
               .OrderBy(m => m.Title)   
                .ToListAsync(cancellationToken)
                .ContinueWith(task => task.Result.AsEnumerable(), cancellationToken);
        }
    }
}
