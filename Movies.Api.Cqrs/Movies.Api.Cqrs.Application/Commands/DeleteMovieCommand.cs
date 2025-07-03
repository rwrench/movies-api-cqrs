// CQRS Command for deleting a movie
using MediatR;

namespace Movies.Api.Cqrs.Application.Commands;

public record DeleteMovieCommand(Guid MovieId, Guid? UserId) : IRequest<bool>;