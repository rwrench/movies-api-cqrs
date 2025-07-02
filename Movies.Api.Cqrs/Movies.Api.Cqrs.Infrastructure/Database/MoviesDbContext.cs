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
    public DbSet<User> Users { get; set; } // Add Users DbSet

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Movie 1 - * MovieRating
        modelBuilder.Entity<MovieRating>()
            .HasOne<Movie>()
            .WithMany()
            .HasForeignKey(r => r.MovieId)
            .OnDelete(DeleteBehavior.NoAction);

     
    }
}