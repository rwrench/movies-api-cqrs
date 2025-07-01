using MediatR;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Repositories;
using Movies.Api.Cqrs.Application.Services;

namespace Movies.Api.Cqrs.Application.Handlers
{
    public class UpdateMovieCommandHandler : 
        IRequestHandler<UpdateMovieCommand, bool>
    {
        readonly IMovieCommandService _service;

        public UpdateMovieCommandHandler(IMovieCommandService service)
        {
            _service = service;

        }

        public async Task<bool> Handle(
            UpdateMovieCommand command,
            CancellationToken cancellationToken)
        {
            return await _service.UpdateAsync(command, cancellationToken);
        }
    }
}
