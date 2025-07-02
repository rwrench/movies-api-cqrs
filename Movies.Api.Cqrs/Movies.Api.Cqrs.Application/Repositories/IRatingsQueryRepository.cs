using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Repositories
{
    public interface IRatingsQueryRepository
    {
        Task<IEnumerable<MovieRatingWithNameDto>> GetAllAsync(CancellationToken token = default);
    }
}
