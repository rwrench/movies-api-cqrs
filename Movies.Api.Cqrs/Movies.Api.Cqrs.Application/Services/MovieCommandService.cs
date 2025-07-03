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
        readonly IValidator<Movie> _movieValidator; 
        readonly IValidator<UpdateMovieCommand> _updateValidator;
        readonly IMapper _mapper;

        public MovieCommandService(
            IMovieCommandRepository movieCommandRepository,
            IValidator<Movie> movieValidator, 
            IValidator<UpdateMovieCommand> updateValidator,
            IMapper mapper)
        {
            _movieCommandRepository = movieCommandRepository;
            _movieValidator = movieValidator; 
            _updateValidator = updateValidator;
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(
            CreateMovieCommand command,
            CancellationToken token = default)
        {
            var movie = _mapper.Map<Movie>(command);
            await _movieValidator.ValidateAndThrowAsync(movie, token);
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
            
            // Validate the update command
            await _updateValidator.ValidateAndThrowAsync(command, token);
            
            // Pass the command data directly to repository
            // This avoids Entity Framework tracking issues while still validating input
            return await _movieCommandRepository.UpdateByIdAsync(
                command.MovieId, 
                command.Title, 
                command.YearOfRelease, 
                command.Genres, 
                token);
        }
    }
}
