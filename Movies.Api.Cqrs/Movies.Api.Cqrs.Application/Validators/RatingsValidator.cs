using FluentValidation;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Validators;

public class RatingsValidator : AbstractValidator<MovieRating>
{
    IMovieQueryRepository _movieQueryRepo;

    public RatingsValidator(IMovieQueryRepository movieCommandRepository)
    {
        _movieQueryRepo = movieCommandRepository;
       
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5)
            .WithMessage("User rating must be between 1 and 5.");
        RuleFor(x => x.Id)
          .MustAsync(MovieExists)
          .WithMessage("Movie does not exist");
    }

    async Task<bool> MovieExists(
       Guid movieId,
       CancellationToken token)
    {

        var movie = await _movieQueryRepo.GetByIdAsync(movieId);
        return movie is not null;
    }




}
