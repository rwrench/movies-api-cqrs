using MediatR;

namespace Movies.CQRS.Commands;

public record UpdateMovieCommand(
    Guid Id,
    string Title, 
    int YearOfRelease, 
    List<string> Genres,
    Guid? UserId) : IRequest<bool>;
