namespace Movies.Application;

using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Repositories;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MoveRepository>();
        
        return services;
    }
}