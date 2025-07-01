using AutoMapper;
using MediatR;
using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Application.Handlers;


public class GetAllMoviesQueryHandler : IRequestHandler<GetAllMoviesQuery, List<MovieDto>>
{
    readonly IMovieQueryRepository _movieRepository;
    readonly IMapper _mapper;

    public GetAllMoviesQueryHandler(IMovieQueryRepository movieRepository, IMapper mapper)
    {
        _movieRepository = movieRepository;
        _mapper = mapper;
    }

    public async Task<List<MovieDto>> Handle(GetAllMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await _movieRepository.GetAllAsync(cancellationToken);
        return _mapper.Map<List<MovieDto>>(movies);
    }
}
