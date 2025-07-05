using MediatR;
using Movies.Api.Contracts.Dto;

namespace Movies.Api.Cqrs.Application.Queries;

public record GetMovieByIdQuery(Guid Id, Guid? UserId) : IRequest<MovieDto>;
