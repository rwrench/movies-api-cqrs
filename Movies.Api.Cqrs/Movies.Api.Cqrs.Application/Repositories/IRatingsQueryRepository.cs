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
        Task<IEnumerable<MovieRating?>> GetAllAsync(CancellationToken token = default);
    }
}
