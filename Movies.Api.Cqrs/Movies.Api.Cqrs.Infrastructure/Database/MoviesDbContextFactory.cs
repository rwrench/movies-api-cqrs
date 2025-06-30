using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.Hosting;

namespace Movies.Api.Cqrs.Infrastructure.Database
{
    public class MoviesDbContextFactory : IDesignTimeDbContextFactory<MoviesDbContext>
    {
        public MoviesDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Ensure Microsoft.Extensions.Hosting is referenced
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<MoviesDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")); // Ensure Microsoft.EntityFrameworkCore.SqlServer is installed

            return new MoviesDbContext(optionsBuilder.Options);
        }
    }
}
