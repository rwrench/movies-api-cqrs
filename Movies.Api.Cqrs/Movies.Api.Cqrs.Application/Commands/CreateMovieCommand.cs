using MediatR;
using System.Net;

namespace Movies.Api.Cqrs.Application.Commands;

public record CreateMovieCommand(
    string Title,
    int YearOfRelease,
    List<string> Genres, 
    Guid? UserId) : IRequest<Guid>;
