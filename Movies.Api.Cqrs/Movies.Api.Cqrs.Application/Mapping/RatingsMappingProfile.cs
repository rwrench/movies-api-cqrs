using Movies.Api.Cqrs.Application.Dto;
using Movies.Api.Cqrs.Application.Models;
using AutoMapper;


namespace Movies.Api.Cqrs.Application.Mapping
{
    public class RatingsMappingProfile : Profile // Ensure RatingsMappingProfile inherits from AutoMapper.Profile
    {
        public RatingsMappingProfile() // Add a constructor to define mappings
        {
            CreateMap<MovieRating, RatingsDto>(); // Ensure MovieRatingDto exists in the correct namespace
        }
    }
}
