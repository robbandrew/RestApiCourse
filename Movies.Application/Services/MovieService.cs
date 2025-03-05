namespace Movies.Application.Services;

using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IValidator<Movie> _movieValidator;

    public MovieService(IMovieRepository movieRepository, IValidator<Movie> movieValidator)
    {
        _movieRepository = movieRepository;
        _movieValidator = movieValidator;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken);
        return await _movieRepository.CreateAsync(movie, cancellationToken);
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _movieRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _movieRepository.GetBySlugAsync(slug, cancellationToken);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _movieRepository.GetAllAsync(cancellationToken);
    }

    public async Task<Movie?> UpdateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        await _movieValidator.ValidateAndThrowAsync(movie, cancellationToken);
        var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id, cancellationToken);
        if (!movieExists) return null;

        await _movieRepository.UpdateAsync(movie);
        return movie;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _movieRepository.DeleteAsync(id, cancellationToken);
    }
}