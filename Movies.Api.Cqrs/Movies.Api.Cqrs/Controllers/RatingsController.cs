using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Contracts.Dto;

namespace Movies.Api.Cqrs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : Controller
{
    readonly IMediator _mediator;
    readonly ILogger<RatingsController> _logger;

    public RatingsController(IMediator mediator, ILogger<RatingsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateRatingDto dto,
        CancellationToken token)
    {
        _logger.LogInformation("Create rating called for movie: {MovieId}, Rating: {Rating}, UserId: {UserId}", 
            dto.MovieId, dto.Rating, dto.UserId);
            
        try
        {
            // Validate the DTO
            if (dto.MovieId == Guid.Empty)
            {
                _logger.LogWarning("MovieId is empty");
                return BadRequest("MovieId is required");
            }

            if (dto.Rating <= 0 || dto.Rating > 10)
            {
                _logger.LogWarning("Invalid rating value: {Rating}", dto.Rating);
                return BadRequest("Rating must be between 0 and 10");
            }

            // Create the command from the DTO
            var command = new CreateRatingCommand(dto.MovieId, dto.Rating, dto.UserId);
            
            var ratingId = await _mediator.Send(command, token);
            _logger.LogInformation("Successfully created rating with ID: {RatingId}", ratingId);
            return Ok(new { id = ratingId, success = true, message = "Rating created successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating rating for movie: {MovieId}", dto.MovieId);
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] RatingsDto dto,
        CancellationToken token)
    {
        _logger.LogInformation("Update rating called with ID: {Id}", id);
        
        if (dto == null)
        {
            _logger.LogWarning("RatingsDto is null");
            return BadRequest("Invalid request body");
        }

        try
        {
            var command = new UpdateRatingCommand(
                id,
                dto.MovieId,
                dto.Rating,
                dto.UserId
            );
            
            var result = await _mediator.Send(command, token);
            
            if (result)
            {
                return Ok(new { success = true, message = "Rating updated successfully" });
            }
            else
            {
                return NotFound(new { success = false, message = "Rating not found or update failed" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating rating with ID: {Id}", id);
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpPut]
    public async Task<IActionResult> RateMovie(
        [FromQuery] RateMovieCommand command,
         CancellationToken token)
    {
       
       var result = await _mediator.Send(command, token);
       return result
            ? Ok()
            : BadRequest("Failed to rate movie");
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
       CancellationToken token)
    {
        var ratings = await _mediator.Send(new GetAllRatingsQuery(), token);
        return Ok(ratings);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
     Guid id,
     CancellationToken token)
    {
        _logger.LogInformation("Delete rating called with ID: {Id}", id);
        var result = await _mediator.Send(new DeleteRatingCommand(id, null), token); 
        
        if (result)
        {
            return Ok(new { success = true, message = "Rating deleted successfully" });
        }
        else
        {
            return NotFound(new { success = false, message = "Rating not found" });
        }
    }
}