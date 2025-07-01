using AutoMapper;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Models;



public class MovieMappingProfile : Profile
{
    public MovieMappingProfile()
    {
        CreateMap<Movie, MovieDto>();
        CreateMap<CreateMovieCommand, Movie>();
        CreateMap<UpdateMovieCommand, Movie>();
    }
}
