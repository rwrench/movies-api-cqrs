using FluentValidation;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;


namespace Movies.Api.Cqrs.Application.Services
{
    public class RatingsMovieCommandService : IRatingsCommandService
    {
        readonly IMovieQueryRepository _movieQueryRepo; 
        readonly IRatingsCommandRepository _repo;
        readonly IValidator<RateMovieCommand> _validator;

        public RatingsMovieCommandService(
            IMovieQueryRepository movieQueryRepo,
            IRatingsCommandRepository repo, 
            IValidator<RateMovieCommand> validator)
        {
            _movieQueryRepo = movieQueryRepo;
            _repo = repo;
            _validator = validator;
        }

        public async Task<bool> RateMovieAsync(
            RateMovieCommand command, 
            CancellationToken token = default)
        {
           await _validator.ValidateAndThrowAsync(command, token);

            return await _repo.RateMovieAsync(
                command.MovieId, 
                command.Rating,
                command.UserId,
                token);
        }
    }
}
