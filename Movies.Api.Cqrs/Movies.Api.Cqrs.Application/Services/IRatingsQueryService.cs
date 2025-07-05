using Movies.Api.Contracts.Dto;

namespace Movies.Api.Cqrs.Application.Services;

public interface IRatingsQueryService
{
    Task<IEnumerable<MovieRatingWithNameDto>>
        GetAllAsync(CancellationToken token = default);
}
