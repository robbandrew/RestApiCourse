namespace Movies.Application.Services;

using Movies.Application.Models;
using Movies.Application.Repositories;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;

    public MovieService(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    public async Task<bool> CreateAsync(Movie movie)
    {
        return await this._movieRepository.CreateAsync(movie);
    }

    public async Task<Movie?> GetByIdAsync(Guid id)
    {
        return await this._movieRepository.GetByIdAsync(id);
    }

    public async Task<Movie?> GetBySlugAsync(string slug)
    {
        return await this._movieRepository.GetBySlugAsync(slug);
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        return await this._movieRepository.GetAllAsync();
    }

    public async Task<Movie?> UpdateAsync(Movie movie)
    {
       var movieExists = await _movieRepository.ExistsByIdAsync(movie.Id);
       if (!movieExists) return null;
       
       await this._movieRepository.UpdateAsync(movie);
       return movie;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await this._movieRepository.DeleteAsync(id);
    }
}