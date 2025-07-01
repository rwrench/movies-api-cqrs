using MediatR;

namespace Movies.Api.Cqrs.Application.Commands;

public record RateMovieCommand(
    Guid MovieId, 
    float Rating,
    Guid UserId) : IRequest<bool>;

