namespace Movies.Contracts.Responses;

public sealed record MovieResponse
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string Slug { get; init; }
    public float? Rating { get ; init; }
    public int? UserRating { get; init; }
    public required int YearOfRelease { get; init; }
    public required IEnumerable<string> Genres { get; init; } = [];
}

public sealed record MovieRatingResponse
{
    public required Guid MovieId { get; init; }
    public required string Slug { get; init; }
    public required int Rating { get; init; }
}