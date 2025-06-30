using MediatR;
using Movies.Api.Cqrs.Application.Repositories;
using Movies.CQRS.Commands;

namespace Movies.Api.Cqrs.Application.Handlers
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, bool>
    {
        private readonly IMovieCommandRepository _repository;

        public DeleteMovieCommandHandler(IMovieCommandRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            var movie = await _repository.GetByIdAsync(request.Id);
            if (movie is null) return false;

            await _repository.DeleteAsync(movie.Id);
            return true;
        }
    }
}
