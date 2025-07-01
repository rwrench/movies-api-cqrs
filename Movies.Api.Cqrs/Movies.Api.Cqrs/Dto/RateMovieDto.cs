namespace Movies.Api.Cqrs.Dto
{
    public record RateMovieDto(
      Guid MovieId,
      int Rating,
      Guid UserId
  );
}
