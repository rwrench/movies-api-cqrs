using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Dto;

namespace Movies.Api.Cqrs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RatingsController : Controller
{
    readonly IMediator _mediator;

    public RatingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPut("{movioId:guid}")]
    public async Task<IActionResult> RateMovie(
         [FromBody] RateMovieDto dto,
         CancellationToken token)
    {
        var command = new RateMovieCommand (
            dto.MovieId,
            dto.Rating,
            dto.UserId
        );
       var result = await _mediator.Send(command, token);
       return result
            ? Ok()
            : BadRequest("Failed to rate movie");
    }


}