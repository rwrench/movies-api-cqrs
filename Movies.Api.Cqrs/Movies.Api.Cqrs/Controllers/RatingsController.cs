using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Queries;
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
        var movies = await _mediator.Send(new GetAllRatingsQuery(), token);
        return Ok(movies);
    }
}