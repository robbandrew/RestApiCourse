﻿namespace Movies.Application.Repositories;

using Movies.Application.Models;

public interface IMovieRepository
{
    Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken cancellationToken = default);
    Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken cancellationToken = default);
    Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(Movie movie, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default);
}