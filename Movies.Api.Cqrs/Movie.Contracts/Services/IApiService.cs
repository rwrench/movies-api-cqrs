

namespace Movies.Api.Contracts.Services
{
    public interface IApiService
    {
        string GetMoviesEndpoint();
        string GetRatingsEndpoint();
        string GetMovieSearchEndpoint(string searchTerm, int take = 10);
        string GetMovieByIdEndpoint(Guid movieId);
        string GetRatingByIdEndpoint(Guid ratingId);
    }
}