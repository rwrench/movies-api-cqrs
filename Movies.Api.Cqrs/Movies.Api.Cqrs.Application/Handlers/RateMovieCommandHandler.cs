using MediatR;
using Movies.Api.Cqrs.Application.Commands;
using Movies.Api.Cqrs.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Handlers
{
    public class RateMovieCommandHandler : IRequestHandler<RateMovieCommand, bool>
    {
        IRatingsCommandService _service;

        public RateMovieCommandHandler(IRatingsCommandService service)
        {
            _service = service;
        }

        public async Task<bool> Handle(
            RateMovieCommand command, 
            CancellationToken cancellationToken)
        {
           return await _service.RateMovieAsync(command, cancellationToken);
        }
    }
}
