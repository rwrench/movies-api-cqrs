using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Application.Repositories;

public interface IMovieCommandRepository
{
    Task<Guid> CreateAsync(Movie movie, CancellationToken token);
    Task<bool> UpdateByIdAsync(Guid id, string title, int yearOfRelease, List<string> genres, CancellationToken token);
    Task<bool> DeleteAsync(
        Guid id, 
        Guid? userId = null, 
        CancellationToken token = default);
}
