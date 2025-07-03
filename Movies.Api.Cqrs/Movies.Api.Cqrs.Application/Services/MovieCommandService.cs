using AutoMapper;
using FluentValidation;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;

namespace Movies.Api.Cqrs.Application.Services
{
    public class MovieCommandService : IMovieCommandService
    {
        readonly IMovieCommandRepository _movieCommandRepository;
        readonly IValidator<Movie> _validator; 
        readonly IMapper _mapper;

        public MovieCommandService(
            IMovieCommandRepository movieCommandRepository,
            IValidator<Movie> validator, 
            IMapper mapper)
        {
            _movieCommandRepository = movieCommandRepository;
            _validator = validator; 
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(
            CreateMovieCommand command,
            CancellationToken token = default)
        {
            var movie = _mapper.Map<Movie>(command);
            await _validator.ValidateAndThrowAsync(movie, token);
            return await _movieCommandRepository.CreateAsync(movie, token);
        }

        public async Task<bool> DeleteAsync(DeleteMovieCommand command, 
            CancellationToken token = default)
        {
            return await _movieCommandRepository.DeleteAsync(command.MovieId, command.UserId, token);
        }

        public async Task<bool> UpdateAsync(UpdateMovieCommand command, 
            CancellationToken token = default)
        {
            Console.WriteLine($"MovieCommandService.UpdateAsync called with MovieId: {command.MovieId}");
            
            // Don't use AutoMapper for updates - pass the command data directly to repository
            // This avoids Entity Framework tracking issues and validation problems
            return await _movieCommandRepository.UpdateByIdAsync(
                command.MovieId, 
                command.Title, 
                command.YearOfRelease, 
                command.Genres, 
                token);
        }
    }
}
