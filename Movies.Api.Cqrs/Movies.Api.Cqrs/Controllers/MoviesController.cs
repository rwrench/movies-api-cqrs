using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Queries;

namespace Movies.Api.Cqrs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : Controller
{
    readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMovieCommand command,
        CancellationToken token)
    {
        var movieId = await _mediator.Send(command,token);
        return Ok(movieId);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllMoviesOptions options,
        CancellationToken token)
    {
        var movies = await _mediator.Send(new GetAllMoviesQuery(options), token);
        return Ok(movies);
    }

}
