using CsvHelper;
using MediatR;
using Movies.Api.Contracts.Dto;
using Movies.Api.Contracts.Models;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Dto;
using System.Globalization;

namespace Movies.Api.Cqrs.Endpoints;

public static class MoviesEndpoints
{
    public static void MapMoviesEndpoints(this WebApplication app)
    {
        var moviesGroup = app.MapGroup("api/movies").WithTags("Movies");

        moviesGroup.MapPost("/", CreateMovie);
        moviesGroup.MapPut("/{id:guid}", UpdateMovie);
        moviesGroup.MapGet("/", GetAllMovies);
        moviesGroup.MapGet("/{id:guid}", GetMovieById);
        moviesGroup.MapGet("/slug/{slug}", GetMovieBySlug);
        moviesGroup.MapDelete("/{id:guid}", DeleteMovie);
        moviesGroup.MapPost("/import", ImportMovies);
        moviesGroup.MapGet("/search", SearchMovies);
    }

    private static async Task<IResult> CreateMovie(
        CreateMovieCommand command, 
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token)
    {
        logger.LogInformation("Create movie called with title: {Title}", command.Title);
        var movieId = await mediator.Send(command, token);
        return Results.Ok(movieId);
    }

    private static async Task<IResult> UpdateMovie(
        Guid id, 
        UpdateMovieDto dto, 
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token)
    {
        logger.LogInformation("Update movie called with ID: {Id}, Title: {Title}", id, dto?.Title);
        
        if (dto == null)
        {
            logger.LogWarning("UpdateMovieDto is null");
            return Results.BadRequest("Invalid request body");
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            logger.LogWarning("Title is null or empty");
            return Results.BadRequest("Title is required");
        }

        if (dto.Genres == null)
        {
            logger.LogWarning("Genres is null, setting to empty list");
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
            
            logger.LogInformation("Sending UpdateMovieCommand to mediator");
            var result = await mediator.Send(command, token);
            
            logger.LogInformation("Update result: {Result}", result);
            
            if (result)
            {
                return Results.Ok(new { success = true, message = "Movie updated successfully" });
            }
            else
            {
                return Results.NotFound(new { success = false, message = "Movie not found or update failed" });
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating movie with ID: {Id}", id);
            return Results.Problem(detail: ex.Message, statusCode: 500);
        }
    }

    private static async Task<IResult> GetAllMovies(
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token, 
        [AsParameters] GetAllMoviesOptions options)
    {
        logger.LogInformation("GetAll movies called");
        var movies = await mediator.Send(new GetAllMoviesQuery(options), token);
        return Results.Ok(movies);
    }

    private static async Task<IResult> GetMovieById(
        Guid id, 
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token)
    {
        logger.LogInformation("GetById called with ID: {Id}", id);
        var movie = await mediator.Send(new GetMovieByIdQuery(id, null), token);
        
        if (movie == null)
        {
            return Results.NotFound(new { message = "Movie not found" });
        }
        
        return Results.Ok(movie);
    }

    private static async Task<IResult> GetMovieBySlug(
        string slug, 
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token)
    {
        logger.LogInformation("GetBySlug called with slug: {Slug}", slug);
        var movie = await mediator.Send(new GetMovieBySlugQuery(slug, null), token);
        
        if (movie == null)
        {
            return Results.NotFound(new { message = "Movie not found" });
        }
        
        return Results.Ok(movie);
    }

    private static async Task<IResult> DeleteMovie(
        Guid id, 
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token)
    {
        logger.LogInformation("Delete called with ID: {Id}", id);
        var result = await mediator.Send(new DeleteMovieCommand(id, null), token);
        
        if (result)
        {
            return Results.Ok(new { success = true, message = "Movie deleted successfully" });
        }
        else
        {
            return Results.NotFound(new { success = false, message = "Movie not found" });
        }
    }

    private static async Task<IResult> ImportMovies(
        IFormFile file, 
        MoviesDbContext dbContext)
    {
        if (file == null || file.Length == 0)
            return Results.BadRequest("No file uploaded.");

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

        return Results.Ok($"{records.Count} movies imported.");
    }

    private static async Task<IResult> SearchMovies(
        IMediator mediator, 
        ILogger<Program> logger, 
        CancellationToken token,
        string searchTerm = "", 
        int take = 15)
    {
        logger.LogInformation("SearchMovies called with searchTerm: {SearchTerm}", searchTerm);
        
        var options = new GetAllMoviesOptions
        {
            Title = searchTerm,
            PageSize = take
        };
        
        var movies = await mediator.Send(new GetAllMoviesQuery(options), token);
        
        var searchResults = movies
            .Where(m => m != null)
            .Select(m => new MovieDropdownDto
            {
                Id = m!.MovieId,
                Title = m.Title,
                YearOfRelease = m.YearOfRelease
            })
            .ToList();
        
        return Results.Ok(searchResults);
    }
}