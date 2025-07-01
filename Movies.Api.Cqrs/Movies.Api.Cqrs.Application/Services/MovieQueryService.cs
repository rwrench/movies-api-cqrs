using FluentValidation;
using Movies.Api.Cqrs.Application.Models;
using Movies.Api.Cqrs.Application.Queries;
using Movies.Api.Cqrs.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Services
{
    public class MovieQueryService : IMovieQueryService
    {
        readonly IMovieQueryRepository _movieQueryRepository;
        readonly IValidator<GetAllMoviesOptions> _optionsValidator;

        public MovieQueryService(
            IMovieQueryRepository movieQueryRepository, 
            IValidator<GetAllMoviesOptions> optionsValidator)
        {
            _movieQueryRepository = movieQueryRepository;
            _optionsValidator = optionsValidator;
        }

        public async Task<IEnumerable<Movie?>> GetAllAsync(
            GetAllMoviesQuery query, 
            CancellationToken token = default)
        {
            
            await _optionsValidator.ValidateAndThrowAsync(query.options, token);
            return await _movieQueryRepository.GetAllAsync(query.options, token);
        }

        public async Task<Movie?> GetByIdAsync(GetMovieByIdQuery query, CancellationToken token = default)
        {
           return await _movieQueryRepository.GetByIdAsync(query.Id, query.UserId, token);
        }

        public Task<Movie?> GetBySlugAsync(GetMovieBySlugQuery query, CancellationToken token = default)
        {
            throw new NotImplementedException();
        }
    }
}
