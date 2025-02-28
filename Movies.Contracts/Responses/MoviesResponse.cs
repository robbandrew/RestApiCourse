namespace Movies.Contracts.Responses;

public sealed record MoviesResponse
{
    public required IEnumerable<MovieResponse> Movies { get; set; } = [];
}