using MediatR;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Repositories;
using Movies.Api.Cqrs.Application.Services;


namespace Movies.Api.Cqrs.Application.Handlers
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, bool>
    {
        readonly IMovieCommandService _service;
        public DeleteMovieCommandHandler(IMovieCommandService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(
            DeleteMovieCommand request, 
            CancellationToken cancellationToken)
        {
          return await _service.DeleteAsync(request, cancellationToken);
        }
    }
}
