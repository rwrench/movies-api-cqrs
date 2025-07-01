using AutoMapper;
using FluentValidation;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Repositories;
using Movies.Api.Cqrs.Application.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Services
{
    public class MovieCommandService : IMovieCommandService
    {
        readonly IMovieCommandRepository _movieCommandRepository;
        readonly IValidator<Movie> _validator; // Change type to IValidator<Movie>
        readonly IMapper _mapper;

        public MovieCommandService(
            IMovieCommandRepository movieCommandRepository,
            IValidator<Movie> validator, // Change type to IValidator<Movie>
            IMapper mapper)
        {
            _movieCommandRepository = movieCommandRepository;
            _validator = validator; // Update assignment to match type change
            _mapper = mapper;
        }

        public async Task<Guid> CreateAsync(
            CreateMovieCommand command,
            CancellationToken token = default)
        {
            var movie = _mapper.Map<Movie>(command);
            await _validator.ValidateAndThrowAsync(movie, token); // Remove explicit type argument
            return await _movieCommandRepository.CreateAsync(movie, token);
        }

        public Task<bool> DeleteAsync(DeleteMovieCommand command, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(UpdateMovieCommand command, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
