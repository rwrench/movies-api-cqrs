using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Movies.Api.Contracts.Models;

namespace Movies.Api.Cqrs.Infrastructure.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly MoviesDbContext _context;
    
    public MovieValidator(MoviesDbContext context)
    {
        _context = context;
        
        RuleFor(x => x.MovieId).NotEmpty();
        RuleFor(x => x.Genres).NotEmpty();
        RuleFor(x => x.Title).NotEmpty()
            .WithMessage("Title is required.");
        RuleFor(x => x.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(x => x.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie already exists in the system");
    }

    private async Task<bool> ValidateSlug(
        Movie movie,
        string slug,
        CancellationToken token)
    {
        var existingMovie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Slug == slug, token);
            
        if (existingMovie != null)
        {
            return existingMovie.MovieId == movie.MovieId;
        }
        return true;
    }
}