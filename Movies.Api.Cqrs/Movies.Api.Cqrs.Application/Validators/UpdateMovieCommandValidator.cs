using FluentValidation;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Application.Validators;

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    private readonly IMovieQueryRepository _movieQueryRepo;

    public UpdateMovieCommandValidator(IMovieQueryRepository movieQueryRepo)
    {
        _movieQueryRepo = movieQueryRepo;

        RuleFor(x => x.MovieId)
            .NotEmpty()
            .WithMessage("MovieId is required for updates")
            .MustAsync(MovieExists)
            .WithMessage("Movie does not exist");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.YearOfRelease)
            .GreaterThan(1888) // First movie was made in 1888
            .WithMessage("Year of release must be after 1888")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 5) // Allow future releases up to 5 years
            .WithMessage("Year of release cannot be more than 5 years in the future");

        RuleFor(x => x.Genres)
            .NotNull()
            .WithMessage("Genres list cannot be null")
            .Must(genres => genres.Count > 0)
            .WithMessage("At least one genre is required")
            .Must(genres => genres.Count <= 10)
            .WithMessage("Cannot have more than 10 genres");

        RuleForEach(x => x.Genres)
            .NotEmpty()
            .WithMessage("Genre names cannot be empty")
            .MaximumLength(50)
            .WithMessage("Genre names cannot exceed 50 characters");
    }

    private async Task<bool> MovieExists(Guid movieId, CancellationToken token)
    {
        var movie = await _movieQueryRepo.GetByIdAsync(movieId);
        return movie is not null;
    }
}