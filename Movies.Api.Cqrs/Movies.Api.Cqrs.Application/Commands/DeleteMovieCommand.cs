// CQRS Command for deleting a movie
using MediatR;

namespace Movies.CQRS.Commands;

public record DeleteMovieCommand(Guid Id, Guid? UserId) : IRequest<bool>;