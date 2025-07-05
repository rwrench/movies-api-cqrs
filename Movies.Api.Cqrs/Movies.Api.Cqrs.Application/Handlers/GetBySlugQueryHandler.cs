using AutoMapper;
using MediatR;
using Movies.Api.Contracts.Dto;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Application.Services;


namespace Movies.Api.Cqrs.Application.Handlers;
    
    public class GetBySlugQueryHandler : IRequestHandler<GetMovieBySlugQuery, MovieDto>
    {
        readonly IMovieQueryService _movieQueryService;
        readonly IMapper _mapper;

        public GetBySlugQueryHandler(
            IMovieQueryService movieQueryService,
            IMapper mapper)
        {
            _movieQueryService = movieQueryService;
            _mapper = mapper;
        }

        public async Task<MovieDto> Handle(
            GetMovieBySlugQuery query,
            CancellationToken cancellationToken)
        {
            var movie = await _movieQueryService.GetBySlugAsync(query, cancellationToken);
            return _mapper.Map<MovieDto>(movie);
        }
    }
    

