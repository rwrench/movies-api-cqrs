using MediatR;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Repositories;


namespace Movies.Api.Cqrs.Application.Handlers
{
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, bool>
    {
        private readonly IMovieCommandRepository _repository;

        public UpdateMovieCommandHandler(IMovieCommandRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> Handle(
            UpdateMovieCommand request,
            CancellationToken cancellationToken)
        {

            var movie = await _repository.GetByIdAsync(request.Id);
            if (movie is null) return false;

            movie.Title = request.Title;
            movie.YearOfRelease = request.YearOfRelease;
            movie.Genres = request.Genres;

            return await _repository.UpdateAsync(movie);
        }
    }
}
