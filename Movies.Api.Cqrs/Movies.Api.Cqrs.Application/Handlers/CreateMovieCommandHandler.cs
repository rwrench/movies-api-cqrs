using MediatR;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Services;

namespace Movies.Api.Cqrs.Application.Handlers;

public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, Guid>
{
    private readonly IMovieCommandService _movieCommandService;

    public CreateMovieCommandHandler(IMovieCommandService movieCommandService)
    {
        _movieCommandService = movieCommandService;
    }

    public async Task<Guid> Handle(
        CreateMovieCommand command, 
        CancellationToken token)
    {
       return await _movieCommandService.CreateAsync(command, token);
        
    }
}
