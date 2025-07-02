using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Api.Cqrs.Application.Dto
{
    public class MovieRatingWithNameDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public float Rating { get; set; }
        public Guid? UserId { get; set; }
        public DateTime DateUpdated { get; set; }
        public string MovieName { get; set; } = string.Empty;
    }
}
