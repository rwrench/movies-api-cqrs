using FluentValidation;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Application.Validators;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieQueryRepository _movieRepo;
    public MovieValidator(IMovieQueryRepository movieRepo)
    {
        _movieRepo = movieRepo;
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

        var existingMovie = await _movieRepo.GetBySlugAsync(slug);
        if (existingMovie is not null)
        {
            return existingMovie.MovieId == movie.MovieId;
        }
        return existingMovie is null;
    }
}
