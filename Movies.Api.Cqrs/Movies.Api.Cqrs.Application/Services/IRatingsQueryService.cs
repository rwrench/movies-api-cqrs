using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Models;

namespace Movies.Api.Cqrs.Application.Services;

public interface IRatingsQueryService
{
    Task<IEnumerable<MovieRatingWithNameDto>> GetAllAsync(CancellationToken token = default);
}
