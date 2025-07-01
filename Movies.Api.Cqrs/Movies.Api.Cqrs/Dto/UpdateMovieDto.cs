namespace Movies.Api.Cqrs.Dto
{
    // DTO for update binding (all fields required for PUT)
    public record UpdateMovieDto(
        string Title,
        int YearOfRelease,
        List<string> Genres,
        Guid? UserId
    );
}
