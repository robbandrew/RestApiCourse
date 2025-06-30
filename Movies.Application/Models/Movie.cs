namespace Movies.Application.Models;

using System.Text.RegularExpressions;

public partial class Movie
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }
    public required int YearOfRelease { get; set; }
    public float? Rating { get; set; }
    public float? MasterRating { get; set; }
    public int? UserRating { get; set; }
    public required List<string> Genres { get; set; } = [];

    public string Slug => GenerateSlug();

    private string GenerateSlug()
    {
        var sluggedTitle = SlugRegex().Replace(Title, string.Empty)
            .ToLower().Replace(" ", "-");
        return $"{sluggedTitle}-{YearOfRelease}";
    }

    [GeneratedRegex("[^0-9A-Za-z _-]", RegexOptions.NonBacktracking, 5)]
    private static partial Regex SlugRegex();
}