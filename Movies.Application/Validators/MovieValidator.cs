namespace Movies.Application.Validators;

using FluentValidation;
using Movies.Application.Models;
using Movies.Application.Repositories;

public class MovieValidator : AbstractValidator<Movie>
{
    private readonly IMovieRepository _movieRepository;
    public MovieValidator(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
        RuleFor(m => m.Id)
            .NotEmpty();
        
        RuleFor(m => m.Genres)
            .NotEmpty();
        
        RuleFor(m => m.Title)
            .NotEmpty();
        
        RuleFor(m => m.YearOfRelease)
            .LessThanOrEqualTo(DateTime.UtcNow.Year);

        RuleFor(m => m.Slug)
            .MustAsync(ValidateSlug)
            .WithMessage("This movie alread exists in the system");
    }

    private async Task<bool> ValidateSlug(Movie movie, string slug, CancellationToken cancellationToken = default)
    {
        var existingMovie = await _movieRepository.GetBySlugAsync(slug);

        if (existingMovie is not null)
        {
            return existingMovie.Id == movie.Id;
        }
        
        return existingMovie is  null;
    }
}