using Movies.Api.Cqrs.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Repositories
{
    public interface IRatingsCommandRepository
    {
        Task<bool> RateMovieAsync(
            Guid movieId,
            float rating,
            Guid userId,
            CancellationToken token);


    }
}
