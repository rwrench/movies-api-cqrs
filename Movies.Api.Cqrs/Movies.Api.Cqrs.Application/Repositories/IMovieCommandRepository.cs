using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Application.Repositories;

public interface IMovieCommandRepository
{
    Task<Guid> CreateAsync(Movie movie, CancellationToken token);
    Task<bool> UpdateAsync(Movie movie, CancellationToken token);

    Task<Movie?> GetByIdAsync(
        Guid id, 
        Guid? userId = null, 
        CancellationToken token = default);

    Task<Movie?> GetBySlugAsync(
        string slug, 
        Guid? userId = null, 
        CancellationToken token = default);

    Task<bool> DeleteAsync(
        Guid id, 
        Guid? userId = null, 
        CancellationToken token = default);
}
