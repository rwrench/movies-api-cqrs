using FluentValidation;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Application.Validators;

public class RateMovieCommandValidator : AbstractValidator<RateMovieCommand>
{
    IMovieQueryRepository _movieQueryRepo;

    public RateMovieCommandValidator(
        IMovieQueryRepository movieCommandRepository)
    {
        _movieQueryRepo = movieCommandRepository;

        RuleFor(x => x.MovieId)
            .MustAsync(MovieExists)
            .WithMessage("Movie does not exist");
        RuleFor(x => x.Rating).InclusiveBetween(0.5f, 5.0f);
        RuleFor(x => x.UserId).NotEmpty();
    }
   
    async Task<bool> MovieExists(
       Guid movieId,
       CancellationToken token)
    {

        var movie = await _movieQueryRepo.GetByIdAsync(movieId);
        return movie is not null;
    }




}
