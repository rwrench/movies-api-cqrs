using AutoMapper;
using MediatR;
using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Application.Services;

namespace Movies.Api.Cqrs.Application.Handlers
{
    public class GetAllRatingsQueryHandler : IRequestHandler<GetAllRatingsQuery,List<RatingsDto>>
    {
        private readonly IRatingsQueryService _ratingsQueryService;
        private readonly IMapper _mapper;

        public GetAllRatingsQueryHandler(
            IRatingsQueryService ratingsQueryService,
            IMapper mapper)
        {
           _ratingsQueryService = ratingsQueryService;
            _mapper = mapper;
        }

        public async Task<List<RatingsDto>> Handle(
            GetAllRatingsQuery request, 
            CancellationToken cancellationToken)
        {
            var ratings = await _ratingsQueryService.GetAllAsync(cancellationToken);
            return _mapper.Map<List<RatingsDto>>(ratings);
        }

    
    }
}
