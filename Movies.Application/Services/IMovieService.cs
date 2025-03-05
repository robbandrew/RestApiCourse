namespace Movies.Application.Services;

using Movies.Application.Models;
using Npgsql.Internal;

public interface IMovieService
{
    Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Movie?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Movie?> UpdateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}