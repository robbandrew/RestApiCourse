namespace Movies.Application.Models;

public sealed record MovieRating
{
    public required Guid MovieId { get; init; }
    public required string Slug { get; init; }
    public required int Rating { get; init; }
}
