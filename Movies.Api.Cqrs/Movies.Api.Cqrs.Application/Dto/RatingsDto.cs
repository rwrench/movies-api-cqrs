using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Dto
{
    public class RatingsDto
    {
        public Guid Id { get; init; }
        public Guid MovieId { get; set; }
        public float Rating { get; set; }
        public DateTime DateUpdated { get; set; }
        public Guid? UserId { get; set; }
       
    }
}
