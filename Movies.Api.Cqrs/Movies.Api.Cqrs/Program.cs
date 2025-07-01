using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Movies.Api.Cqrs.Application;
using Movies.Api.Cqrs.Application.Repositories;
using Movies.Api.Cqrs.Application.Services;
using Movies.Api.Cqrs.Application.Validators;
using Movies.Api.Cqrs.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<MoviesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MovieMappingProfile).Assembly);

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(AssemblyMarker).Assembly));
builder.Services.AddScoped<IMovieCommandRepository, MovieCommandRepository>();
builder.Services.AddScoped<IMovieQueryRepository, MovieQueryRepository>();
builder.Services.AddValidatorsFromAssemblyContaining<GetAllMoviesOptionsValidator>();
builder.Services.AddScoped<IMovieQueryService, MovieQueryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();
    db.Database.Migrate();
}
app.Run();
