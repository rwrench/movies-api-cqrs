using Movies.Api.Cqrs.Application.Models;
using AutoMapper;
using Movies.Api.Contracts.Dto;

namespace Movies.Api.Cqrs.Application.Mapping
{
    public class RatingsMappingProfile : Profile // Ensure RatingsMappingProfile inherits from AutoMapper.Profile
    {
        public RatingsMappingProfile() // Add a constructor to define mappings
        {
            CreateMap<MovieRating, RatingsDto>(); // Ensure MovieRatingDto exists in the correct namespace
            
            // Add mapping for the joined entity result
            CreateMap<(MovieRating Rating, Movie Movie), MovieRatingWithNameDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Rating.Id))
                .ForMember(dest => dest.MovieId, opt => opt.MapFrom(src => src.Rating.MovieId))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating.Rating))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Rating.UserId))
                .ForMember(dest => dest.DateUpdated, opt => opt.MapFrom(src => src.Rating.DateUpdated))
                .ForMember(dest => dest.MovieName, opt => opt.MapFrom(src => src.Movie.Title));
        }
    }
}
