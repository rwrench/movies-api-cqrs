using Movies.Api.Cqrs.Application.Commands;

namespace Movies.Api.Cqrs.Application.Services;

public interface IRatingsCommandService
{
    Task<bool> RateMovieAsync(
        RateMovieCommand command, 
        CancellationToken token = default);
}
