using MediatR;
using Movies.Api.Contracts.Dto;

namespace Movies.Api.Cqrs.Application.Queries;

public record GetMovieBySlugQuery(string Slug, Guid? UserId): IRequest<MovieDto>;
