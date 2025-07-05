using MediatR;
using Movies.Api.Contracts.Dto;
using Movies.Api.Contracts.Models;

namespace Movies.Api.Cqrs.Application.Queries;

public record GetAllMoviesQuery(GetAllMoviesOptions options) : IRequest<List<MovieDto>>;
