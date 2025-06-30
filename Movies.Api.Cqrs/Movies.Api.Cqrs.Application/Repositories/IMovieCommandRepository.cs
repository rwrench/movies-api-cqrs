using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Application.Repositories;

public interface IMovieCommandRepository
{
    Task<Guid> CreateAsync(Movie movie);
    Task<bool> UpdateAsync(Movie movie);

    Task<Movie?> GetByIdAsync(Guid id, Guid? userId = null);

    Task<bool> DeleteAsync(Guid id, Guid? userId = null);
}
