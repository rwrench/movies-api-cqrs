namespace Movies.Api.Cqrs.Application.Extensions;

public static class SlugExtensions
{
    public static (string Title, int YearOfRelease)?
        ParseTitleAndYear(this string slug)
    {
        if (string.IsNullOrWhiteSpace(slug))
            return null;

        var lastDash = slug.LastIndexOf('-');
        if (lastDash < 1 || lastDash == slug.Length - 1)
            return null;

        var titlePart = slug.Substring(0, lastDash).Replace("-", " ");
        var yearPart = slug.Substring(lastDash + 1);

        if (int.TryParse(yearPart, out int year))
            return (titlePart, year);

        return null;
    }
}
