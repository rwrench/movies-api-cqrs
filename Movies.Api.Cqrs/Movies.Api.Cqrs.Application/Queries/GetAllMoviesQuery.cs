using Movies.Api.Cqrs.Application.Models;


namespace Movies.Api.Cqrs.Application.Queries;

public record GetAllMoviesQuery(GetAllMoviesOptions Options);
