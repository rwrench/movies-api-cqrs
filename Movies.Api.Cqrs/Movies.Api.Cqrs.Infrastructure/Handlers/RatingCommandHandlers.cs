using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Api.Contracts.Models;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Infrastructure.Database;

namespace Movies.Api.Cqrs.Infrastructure.Handlers;

public class CreateRatingCommandHandler : IRequestHandler<CreateRatingCommand, Guid>
{
    private readonly MoviesDbContext _context;
    private readonly ILogger<CreateRatingCommandHandler> _logger;

    public CreateRatingCommandHandler(MoviesDbContext context, ILogger<CreateRatingCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateRatingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating rating for movie {MovieId}", request.MovieId);

        var rating = new MovieRating
        {
            Id = Guid.NewGuid(),
            MovieId = request.MovieId,
            Rating = request.Rating,
            UserId = request.UserId,
            DateUpdated = DateTime.UtcNow
        };

        _context.Ratings.Add(rating);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Created rating {RatingId} for movie {MovieId}", rating.Id, request.MovieId);
        return rating.Id;
    }
}

public class UpdateRatingCommandHandler : IRequestHandler<UpdateRatingCommand, bool>
{
    private readonly MoviesDbContext _context;
    private readonly ILogger<UpdateRatingCommandHandler> _logger;

    public UpdateRatingCommandHandler(MoviesDbContext context, ILogger<UpdateRatingCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateRatingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating rating {RatingId}", request.Id);

        var rating = await _context.Ratings
            .Where(r => r.Id == request.Id && (request.UserId == null || r.UserId == request.UserId))
            .FirstOrDefaultAsync(cancellationToken);

        if (rating == null)
        {
            _logger.LogWarning("Rating {RatingId} not found", request.Id);
            return false;
        }

        rating.MovieId = request.MovieId;
        rating.Rating = request.Rating;
        rating.DateUpdated = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Updated rating {RatingId}", request.Id);
        return true;
    }
}

public class DeleteRatingCommandHandler : IRequestHandler<DeleteRatingCommand, bool>
{
    private readonly MoviesDbContext _context;
    private readonly ILogger<DeleteRatingCommandHandler> _logger;

    public DeleteRatingCommandHandler(MoviesDbContext context, ILogger<DeleteRatingCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteRatingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting rating {RatingId}", request.Id);

        var rating = await _context.Ratings
            .Where(r => r.Id == request.Id && (request.UserId == null || r.UserId == request.UserId))
            .FirstOrDefaultAsync(cancellationToken);

        if (rating == null)
        {
            _logger.LogWarning("Rating {RatingId} not found", request.Id);
            return false;
        }

        _context.Ratings.Remove(rating);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Deleted rating {RatingId}", request.Id);
        return true;
    }
}