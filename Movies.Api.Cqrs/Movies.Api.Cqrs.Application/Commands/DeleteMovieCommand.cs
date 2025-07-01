// CQRS Command for deleting a movie
using MediatR;

namespace Movies.Api.Cqrs.Application.Commands;

public record DeleteMovieCommand(Guid Id, Guid? UserId) : IRequest<bool>;