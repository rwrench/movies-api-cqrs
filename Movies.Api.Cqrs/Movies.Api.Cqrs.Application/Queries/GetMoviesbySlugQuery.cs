

namespace Movies.Api.Cqrs.Application.Queries;

public record GetMovieBySlugQuery(string Slug, Guid? UserId);
