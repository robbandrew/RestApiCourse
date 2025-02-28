namespace Movies.Application;

using Microsoft.Extensions.DependencyInjection;
using Movies.Application.Database;
using Movies.Application.Repositories;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddSingleton<IMovieRepository, MoveRepository>();
        
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        services.AddSingleton<DbInitializer>();
        return services;
    }
}