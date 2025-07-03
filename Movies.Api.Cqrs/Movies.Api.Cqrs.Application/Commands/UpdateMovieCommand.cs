using MediatR;

namespace Movies.Api.Cqrs.Application.Commands;

public record UpdateMovieCommand(
    Guid MovieId,
    string Title, 
    int YearOfRelease, 
    List<string> Genres,
    Guid? UserId) : IRequest<bool>;
