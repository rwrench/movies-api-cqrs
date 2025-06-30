namespace Movies.Api.Cqrs.Application.Queries;

public record GetMovieByIdQuery(Guid Id, Guid? UserId);
