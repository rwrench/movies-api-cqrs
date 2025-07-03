using FluentValidation;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Movies.Api.Cqrs.Infrastructure.Validators;

public class RateMovieCommandValidator : AbstractValidator<RateMovieCommand>
{
    private readonly MoviesDbContext _context;

    public RateMovieCommandValidator(MoviesDbContext context)
    {
        _context = context;

        RuleFor(x => x.MovieId)
            .MustAsync(MovieExists)
            .WithMessage("Movie does not exist");
        RuleFor(x => x.Rating).InclusiveBetween(0.5f, 5.0f);
        RuleFor(x => x.UserId).NotEmpty();
    }
   
    private async Task<bool> MovieExists(Guid movieId, CancellationToken token)
    {
        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.MovieId == movieId, token);
        return movie != null;
    }
}