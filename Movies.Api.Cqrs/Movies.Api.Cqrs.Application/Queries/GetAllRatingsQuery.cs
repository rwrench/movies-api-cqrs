using MediatR;
using Movies.Api.Contracts.Dto;


namespace Movies.Api.Cqrs.Application.Queries;


public record GetAllRatingsQuery() : IRequest<IEnumerable<MovieRatingWithNameDto>>;
