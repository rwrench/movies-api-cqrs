using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Application.Services;

public interface IRatingsQueryService
{
    Task<IEnumerable<MovieRating?>> GetAllAsync(CancellationToken token = default);
}
