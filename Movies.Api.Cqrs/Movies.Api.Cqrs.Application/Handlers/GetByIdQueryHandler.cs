using AutoMapper;
using MediatR;
using Movies.Api.Contracts.Dto;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Application.Services;

namespace Movies.Api.Cqrs.Application.Handlers
{
    public class GetByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieDto>
    {
        readonly IMovieQueryService _movieQueryService;
        readonly IMapper _mapper;

        public GetByIdQueryHandler(
            IMovieQueryService movieQueryService, 
            IMapper mapper)
        {
            _movieQueryService = movieQueryService;
            _mapper = mapper;
        }

        public async Task<MovieDto> Handle(
            GetMovieByIdQuery query, 
            CancellationToken cancellationToken)
        {
            var movie = await _movieQueryService.GetByIdAsync(query, cancellationToken);
            return _mapper.Map<MovieDto>(movie);
        }
    }
}
