using MediatR;
using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Models;


namespace Movies.Api.Cqrs.Application.Queries;

public record GetAllMoviesQuery(GetAllMoviesOptions options) : IRequest<List<MovieDto>>;
