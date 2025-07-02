using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Application.Services;

public class RatingsQueryService : IRatingsQueryService
{
    readonly IRatingsQueryRepository _ratingsQueryRepository;

    public RatingsQueryService(IRatingsQueryRepository ratingsQueryRepository)
    {
        _ratingsQueryRepository = ratingsQueryRepository;
    }

    public async Task<IEnumerable<MovieRatingWithNameDto>> 
        GetAllAsync(CancellationToken token = default)
    {
        return await _ratingsQueryRepository.GetAllAsync(token);    
    }

 
}
