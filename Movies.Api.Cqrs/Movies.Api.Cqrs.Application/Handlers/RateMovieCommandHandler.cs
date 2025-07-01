using MediatR;
using Movies.Api.Cqrs.Application.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Handlers
{
    public class RateMovieCommandHandler : IRequestHandler<RateMovieCommand, bool>
    {
        public Task<bool> Handle(RateMovieCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
