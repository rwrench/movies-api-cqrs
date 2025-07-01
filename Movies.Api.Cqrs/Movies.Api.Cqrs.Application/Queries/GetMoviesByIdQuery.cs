using MediatR;
using Movies.Api.Cqrs.Application.Dto;

namespace Movies.Api.Cqrs.Application.Queries;

public record GetMovieByIdQuery(Guid Id, Guid? UserId) : IRequest<MovieDto>;
