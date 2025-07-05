using MediatR;

namespace Movies.Api.Cqrs.Application.Commands;

public record CreateRatingCommand(
    Guid MovieId, 
    float Rating,
    Guid? UserId = null) : IRequest<Guid>;

public record UpdateRatingCommand(
    Guid Id,
    Guid MovieId, 
    float Rating,
    Guid? UserId = null) : IRequest<bool>;

public record DeleteRatingCommand(
    Guid Id,
    Guid? UserId = null) : IRequest<bool>;