namespace Movies.Application.Database;

using Dapper;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
                                      create table if not exists Movies (
                                          id UUID primary key,
                                          slug TEXT not null,
                                          title TEXT not null,
                                          yearofrelease integer not null);
                                      """);

        await connection.ExecuteAsync("""
                                      create unique index concurrently if not exists movies_slug_idx
                                      on Movies
                                      using btree(slug);
                                      """);
        
        await connection.ExecuteAsync("""
                                      create table if not exists Genres (
                                        movieId UUID references movies (id),
                                          name TEXT not null);
                                      """);
    }
}