using Movies.Api.Cqrs.Application.Commands;


namespace Movies.Api.Cqrs.Application.Services;

public interface IMovieCommandService
{
    Task<Guid> CreateAsync(CreateMovieCommand command, CancellationToken token = default);
    Task<bool> UpdateAsync(UpdateMovieCommand command, CancellationToken token = default);
    Task<bool> DeleteAsync(DeleteMovieCommand command, CancellationToken token = default);
}
