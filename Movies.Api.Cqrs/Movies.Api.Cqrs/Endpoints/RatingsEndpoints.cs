using MediatR;
using Movies.Api.Contracts.Dto;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Queries;

namespace Movies.Api.Cqrs.Endpoints;

public static class RatingsEndpoints
{
    public static void MapRatingsEndpoints(this WebApplication app)
    {
        var ratingsGroup = app.MapGroup("api/ratings").WithTags("Ratings");

        ratingsGroup.MapPost("/", CreateRating);
        ratingsGroup.MapPut("/{id:guid}", UpdateRating);
        ratingsGroup.MapPut("/rate", RateMovie);
        ratingsGroup.MapGet("/", GetAllRatings);
        ratingsGroup.MapDelete("/{id:guid}", DeleteRating);
    }

    private static async Task<IResult> CreateRating(
        CreateRatingDto dto, 
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token)
    {
        logger.LogInformation("Create rating called for movie: {MovieId}, Rating: {Rating}, UserId: {UserId}", 
            dto.MovieId, dto.Rating, dto.UserId);
            
        try
        {
            // Validate the DTO
            if (dto.MovieId == Guid.Empty)
            {
                logger.LogWarning("MovieId is empty");
                return Results.BadRequest("MovieId is required");
            }

            if (dto.Rating <= 0 || dto.Rating > 10)
            {
                logger.LogWarning("Invalid rating value: {Rating}", dto.Rating);
                return Results.BadRequest("Rating must be between 0 and 10");
            }

            // Create the command from the DTO
            var command = new CreateRatingCommand(dto.MovieId, dto.Rating, dto.UserId);
            
            var ratingId = await mediator.Send(command, token);
            logger.LogInformation("Successfully created rating with ID: {RatingId}", ratingId);
            return Results.Ok(new { id = ratingId, success = true, message = "Rating created successfully" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating rating for movie: {MovieId}", dto.MovieId);
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    private static async Task<IResult> UpdateRating(
        Guid id, 
        RatingsDto dto, 
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token)
    {
        logger.LogInformation("Update rating called with ID: {Id}", id);
        
        if (dto == null)
        {
            logger.LogWarning("RatingsDto is null");
            return Results.BadRequest("Invalid request body");
        }

        try
        {
            var command = new UpdateRatingCommand(
                id,
                dto.MovieId,
                dto.Rating,
                dto.UserId
            );
            
            var result = await mediator.Send(command, token);
            
            if (result)
            {
                return Results.Ok(new { success = true, message = "Rating updated successfully" });
            }
            else
            {
                return Results.NotFound(new { success = false, message = "Rating not found or update failed" });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating rating with ID: {Id}", id);
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    private static async Task<IResult> RateMovie(
        [AsParameters] RateMovieCommand command, 
        IMediator mediator, 
        CancellationToken token)
    {
        var result = await mediator.Send(command, token);
        return result
            ? Results.Ok()
            : Results.BadRequest("Failed to rate movie");
    }

    private static async Task<IResult> GetAllRatings(
        IMediator mediator, 
        CancellationToken token)
    {
        var ratings = await mediator.Send(new GetAllRatingsQuery(), token);
        return Results.Ok(ratings);
    }

    private static async Task<IResult> DeleteRating(
        Guid id, 
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token)
    {
        logger.LogInformation("Delete rating called with ID: {Id}", id);
        var result = await mediator.Send(new DeleteRatingCommand(id, null), token);
        
        if (result)
        {
            return Results.Ok(new { success = true, message = "Rating deleted successfully" });
        }
        else
        {
            return Results.NotFound(new { success = false, message = "Rating not found" });
        }
    }
}