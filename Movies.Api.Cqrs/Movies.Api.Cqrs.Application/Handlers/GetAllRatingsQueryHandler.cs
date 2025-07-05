using AutoMapper;
using MediatR;
using Movies.Api.Contracts.Dto;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Application.Services;

namespace Movies.Api.Cqrs.Application.Handlers
{
    public class GetAllRatingsQueryHandler : 
        IRequestHandler<GetAllRatingsQuery,IEnumerable<MovieRatingWithNameDto>>
    {
        private readonly IRatingsQueryService _ratingsQueryService;

        public GetAllRatingsQueryHandler(
            IRatingsQueryService ratingsQueryService)
        {
           _ratingsQueryService = ratingsQueryService;
        }

        public async Task<IEnumerable<MovieRatingWithNameDto>> Handle(
            GetAllRatingsQuery request, 
            CancellationToken cancellationToken)
        {
            return await _ratingsQueryService.GetAllAsync(cancellationToken);
        }

    
    }
}
