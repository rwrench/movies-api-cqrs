using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Application.Repositories
{
    public interface IMovieQueryRepository
    {
        Task<IEnumerable<Movie?>> GetAllAsync(GetAllMoviesOptions options, 
            CancellationToken cancellationToken = default);

        Task<Movie?> GetByIdAsync(Guid id, Guid? userId = null,
          CancellationToken cancellationToken = default);
    }
}
