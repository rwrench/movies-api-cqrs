using Microsoft.EntityFrameworkCore;
using Movies.Api.Cqrs.Application.Models;
using System.Collections.Generic;

public class MoviesDbContext : DbContext
{
    public MoviesDbContext(DbContextOptions<MoviesDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }

    public DbSet<MovieRating> Ratings { get; set; }


}