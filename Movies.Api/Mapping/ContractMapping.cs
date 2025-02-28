namespace Movies.Api.Mapping;

using Movies.Application.Models;
using Movies.Contracts.Requests;
using Movies.Contracts.Responses;

public static class ContractMapping
{
    public static Movie MapToMovie(this CreateMovieRequest request)
    {
        return new Movie
        {
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
            Id = Guid.NewGuid(),
        };
    }
    
    public static Movie MapToMovie(this UpdateMovieRequest request, Guid id)
    {
        return new Movie
        {
            Title = request.Title,
            YearOfRelease = request.YearOfRelease,
            Genres = request.Genres.ToList(),
            Id = id,
        };
    }

    public static MovieResponse MapToMovieResponse(this Movie movie)
    {
        return new MovieResponse
        {
            Id = movie.Id,
            Title = movie.Title,
            Slug = movie.Slug,
            YearOfRelease = movie.YearOfRelease,
            Genres = movie.Genres
        };
    }

    public static MoviesResponse MapToMoviesResponse(this IEnumerable<Movie> movies)
    {
        return new MoviesResponse
        {
            Movies = movies.Select(MapToMovieResponse)
        };
    }
}