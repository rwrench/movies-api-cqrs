using AutoMapper;
using Movies.Api.Contracts.Dto;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;

public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<Movie, MovieDto>();

        CreateMap<CreateMovieCommand, Movie>()
            .ForMember(dest => dest.MovieId, opt => opt.MapFrom(_ => Guid.NewGuid()));

        // UpdateMovieCommand now uses MovieId directly, so mapping is straightforward
        CreateMap<UpdateMovieCommand, Movie>();
    }
}
