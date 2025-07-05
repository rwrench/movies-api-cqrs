using CsvHelper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Dto;
using System.Globalization;
using Movies.Api.Cqrs.Application.Dto;

namespace Movies.Api.Cqrs.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MoviesController : Controller
{
    readonly IMediator _mediator;
    readonly ILogger<MoviesController> _logger;

    public MoviesController(IMediator mediator, ILogger<MoviesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateMovieCommand command,
        CancellationToken token)
    {
        _logger.LogInformation("Create movie called with title: {Title}", command.Title);
        var movieId = await _mediator.Send(command, token);
        return Ok(movieId);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateMovieDto dto,
        CancellationToken token)
    {
        _logger.LogInformation("Update movie called with ID: {Id}, Title: {Title}", id, dto?.Title);
        
        // Add debugging
        Console.WriteLine($"UPDATE ENDPOINT HIT - ID: {id}");
        
        if (dto == null)
        {
            _logger.LogWarning("UpdateMovieDto is null");
            return BadRequest("Invalid request body");
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            _logger.LogWarning("Title is null or empty");
            return BadRequest("Title is required");
        }

        if (dto.Genres == null)
        {
            _logger.LogWarning("Genres is null, setting to empty list");
            dto = dto with { Genres = new List<string>() };
        }

        try
        {
            var command = new UpdateMovieCommand(
                id,
                dto.Title,
                dto.YearOfRelease,
                dto.Genres,
                dto.UserId
            );
            
            _logger.LogInformation("Sending UpdateMovieCommand to mediator");
            var result = await _mediator.Send(command, token);
            
            _logger.LogInformation("Update result: {Result}", result);
            
            if (result)
            {
                return Ok(new { success = true, message = "Movie updated successfully" });
            }
            else
            {
                return NotFound(new { success = false, message = "Movie not found or update failed" });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating movie with ID: {Id}", id);
            return StatusCode(500, new { success = false, message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] GetAllMoviesOptions options,
        CancellationToken token)
    {
        _logger.LogInformation("GetAll movies called");
        var movies = await _mediator.Send(new GetAllMoviesQuery(options), token);
        return Ok(movies);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
       Guid id,
       CancellationToken token)
    {
        _logger.LogInformation("GetById called with ID: {Id}", id);
        var movie = await _mediator.Send(new GetMovieByIdQuery(id, null), token);
        
        if (movie == null)
        {
            return NotFound(new { message = "Movie not found" });
        }
        
        return Ok(movie);
    }

    [HttpGet("slug/{slug}")]
    public async Task<IActionResult> GetBySlug(
      string slug,
      CancellationToken token)
    {
        _logger.LogInformation("GetBySlug called with slug: {Slug}", slug);
        var movie = await _mediator.Send(new GetMovieBySlugQuery(slug, null), token);
        
        if (movie == null)
        {
            return NotFound(new { message = "Movie not found" });
        }
        
        return Ok(movie);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
     Guid id,
     CancellationToken token)
    {
        _logger.LogInformation("Delete called with ID: {Id}", id);
        var result = await _mediator.Send(new DeleteMovieCommand(id, null), token); 
        
        if (result)
        {
            return Ok(new { success = true, message = "Movie deleted successfully" });
        }
        else
        {
            return NotFound(new { success = false, message = "Movie not found" });
        }
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

    [HttpGet("search")]
    public async Task<IActionResult> SearchMovies(
        [FromQuery] string searchTerm = "", 
        [FromQuery] int take = 15,
        CancellationToken token = default)
    {
        _logger.LogInformation("SearchMovies called with searchTerm: {SearchTerm}", searchTerm);
        
        var options = new GetAllMoviesOptions
        {
            Title = searchTerm,
            PageSize = take
        };
        
        var movies = await _mediator.Send(new GetAllMoviesQuery(options), token);
        
        var searchResults = movies
            .Where(m => m != null)
            .Select(m => new MovieDropdownDto
            {
                Id = m!.MovieId,
                Title = m.Title,
                YearOfRelease = m.YearOfRelease
            })
            .ToList();
        
        return Ok(searchResults);
    }
}


