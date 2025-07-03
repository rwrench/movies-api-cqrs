using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Dto;
using System.Globalization;

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
        var movieId = await _mediator.Send(command, token);
        return Ok(movieId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMovieDto dto,
        CancellationToken token)
    {
        var command = new UpdateMovieCommand(
            id,
            dto.Title!,
            dto.YearOfRelease,
            dto.Genres!,
            dto.UserId
        );
        var result = await _mediator.Send(command, token);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllMoviesOptions options,
        CancellationToken token)
    {
        var movies = await _mediator.Send(new GetAllMoviesQuery(options), token);
        return Ok(movies);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
       Guid id,
       CancellationToken token)
    {
        var movies = await _mediator.Send(new GetMovieByIdQuery(id, null), token);
        return Ok(movies);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(
      string slug,
      CancellationToken token)
    {
        var movies = await _mediator.Send(new GetMovieBySlugQuery(slug, null), token);
        return Ok(movies);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
     Guid id,
     CancellationToken token)
    {
        var movies = await _mediator.Send(new DeleteMovieCommand(id, null), token); 
        return Ok(movies);
    }

    [HttpPost("import")]
    public async Task<IActionResult> 
        ImportMovies(IFormFile file, [FromServices] MoviesDbContext dbContext)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        using var reader = new StreamReader(stream);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        var records = new List<MovieImportDto>();
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            var dto = new MovieImportDto
            {
                Id = csv.GetField<Guid>("Id"),
                Slug = csv.GetField<string>("Slug")!,
                Title = csv.GetField<string>("Title")!,
                YearOfRelease = csv.GetField<int>("YearOfRelease"),
                Genres = Enumerable.Range(0, 10)
                    .Select(i => csv.GetField<string>($"Genres/{i}")!)
                    .Where(g => !string.IsNullOrWhiteSpace(g))
                    .ToList(),
                Links = Enumerable.Range(0, 2)
                    .Select(i => csv.GetField<string>($"Links/{i}")!)
                    .Where(l => !string.IsNullOrWhiteSpace(l))
                    .ToList()
            };
            records.Add(dto);
        }

        // Map DTOs to your Movie entity and save to DB
        foreach (var dto in records)
        {
            var movie = new Movie
            {
                MovieId = dto.Id,
                Title = dto.Title,
                YearOfRelease = dto.YearOfRelease,
                Genres = dto.Genres
                // Map genres and links as needed for your schema
            };
            dbContext.Movies.Add(movie);
            // Handle genres/links relationships if needed
        }
        await dbContext.SaveChangesAsync();

        return Ok($"{records.Count} movies imported.");
    }
}


