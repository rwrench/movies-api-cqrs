using MediatR;
using Movies.Api.Cqrs.Application.Dto;


namespace Movies.Api.Cqrs.Application.Queries;


public record GetAllRatingsQuery() : IRequest<List<RatingsDto>>;
