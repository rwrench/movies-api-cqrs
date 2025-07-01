using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Application.Repositories;

public interface IMovieCommandRepository
{
    Task<Guid> CreateAsync(Movie movie, CancellationToken token);
    Task<bool> UpdateAsync(Movie movie, CancellationToken token);



    Task<bool> DeleteAsync(
        Guid id, 
        Guid? userId = null, 
        CancellationToken token = default);
}
