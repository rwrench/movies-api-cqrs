using AutoMapper;
using MediatR;
using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Application.Services;

namespace Movies.Api.Cqrs.Application.Handlers;

public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, List<MovieDto>>
{
    private readonly IMovieQueryService _movieQueryService;
    private readonly IMapper _mapper;

    public GetAllMoviesQueryHandler(
        IMovieQueryService movieQueryService, 
        IMapper mapper)
    {
        _movieQueryService = movieQueryService;
        _mapper = mapper;
    }

    public async Task<List<MovieDto>> Handle(
        GetAllMoviesQuery request, 
        CancellationToken cancellationToken)
    {
        var movies = await _movieQueryService.GetAllAsync(request, cancellationToken);
        return _mapper.Map<List<MovieDto>>(movies);
    }
}
