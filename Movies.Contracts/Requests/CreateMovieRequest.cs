namespace Movies.Contracts.Requests;

public sealed record CreateMovieRequest
{
    public required string Title { get; init; }
    public required int YearOfRelease { get; init; }
    public required IEnumerable<string> Genres { get; init; } = [];
}

public sealed record RateMovieRequest
{
    public required int Rating { get; init; }
}