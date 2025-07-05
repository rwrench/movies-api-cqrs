using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Contracts.Models;
using Movies.Api.Cqrs.Application;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Services;
using Movies.Api.Cqrs.Application.Validators;
using Movies.Api.Cqrs.Endpoints;
using Movies.Api.Cqrs.Infrastructure.Handlers;

// Import Infrastructure services and validators
using Movies.Api.Cqrs.Infrastructure.Services;
using Movies.Api.Cqrs.Infrastructure.Validators;
using Movies.Api.Cqrs.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MoviesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MovieMappingProfile).Assembly);

// Register MediatR from both Application and Infrastructure assemblies
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly); // Application assembly
    cfg.RegisterServicesFromAssembly(typeof(CreateRatingCommandHandler).Assembly); // Infrastructure assembly
});

builder.Services.AddScoped<IMovieQueryService, Movies.Api.Cqrs.Infrastructure.Services.MovieQueryService>();
builder.Services.AddScoped<IMovieCommandService, Movies.Api.Cqrs.Infrastructure.Services.MovieCommandService>();
builder.Services.AddScoped<IRatingsCommandService, Movies.Api.Cqrs.Infrastructure.Services.RatingsCommandService>();
builder.Services.AddScoped<IRatingsQueryService, Movies.Api.Cqrs.Infrastructure.Services.RatingsQueryService>();

// Register Infrastructure validators that use DbContext directly
builder.Services.AddScoped<IValidator<Movie>, Movies.Api.Cqrs.Infrastructure.Validators.MovieValidator>();
builder.Services.AddScoped<IValidator<UpdateMovieCommand>, Movies.Api.Cqrs.Infrastructure.Validators.UpdateMovieCommandValidator>();
builder.Services.AddScoped<IValidator<RateMovieCommand>, Movies.Api.Cqrs.Infrastructure.Validators.RateMovieCommandValidator>();

// Register remaining Application validators (no DB access needed)
builder.Services.AddValidatorsFromAssemblyContaining<GetAllMoviesOptionsValidator>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors();

// Map API endpoints
app.MapMoviesEndpoints();
app.MapRatingsEndpoints();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
    db.Database.Migrate();
}
app.Run();
