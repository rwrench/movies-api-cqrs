using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Queries;

namespace Movies.Api.Cqrs.Application.Services;

public interface IMovieQueryService
{
    Task<Movie?> GetByIdAsync(GetMovieByIdQuery query, CancellationToken token = default);
    Task<Movie?> GetBySlugAsync(GetMovieBySlugQuery query, CancellationToken token = default);
    Task<IEnumerable<Movie?>> GetAllAsync(GetAllMoviesQuery query, CancellationToken token = default);
}
