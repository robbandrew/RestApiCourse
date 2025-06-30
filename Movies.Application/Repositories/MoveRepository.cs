namespace Movies.Application.Repositories;

using System.Data;
using Dapper;
using Movies.Application.Database;
using Movies.Application.Models;

public class MoveRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MoveRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task<bool> CreateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        var result = await connection.ExecuteAsync(new CommandDefinition("""
                                                                         insert into movies (id, slug, title, yearofrelease)
                                                                         values (@Id, @Slug, @Title, @YearOfRelease)
                                                                         """, movie, cancellationToken: cancellationToken));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await connection.ExecuteAsync(new CommandDefinition("""
                                                                     
                                                                    insert into genres (movieId, name) 
                                                                    values (@MovieId, @Name)
                                                                    """, new { MovieId = movie.Id, Name = genre }, cancellationToken: cancellationToken));
            }
        }

        transaction.Commit();
        return result > 0;
    }

    public async Task<Movie?> GetByIdAsync(Guid id, Guid? userId = default, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
            select m.*, myr.rating as UserRating, round(avg(r.rating), 1) as MasterRating
            from movies m
            left join ratings r on m.id = r.movieid
            left join ratings myr on m.id = myr.movieid
                and myr.userid = @userId
            where m.id = @id
            group by m.id, m.title, m.yearofrelease, myr.rating
            """, new { id, userId }, cancellationToken: cancellationToken));


        if (movie is null) return null;

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                   
                                  select name from genres where movieId = @id
                                  """, new { id }, cancellationToken: cancellationToken));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug, Guid? userId = default, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var movie = await connection.QuerySingleOrDefaultAsync<Movie>(
            new CommandDefinition("""
                                  select m.*, round(avg(r.rating), 1) as rating, myr.rating as userrating 
                                  from Movies m 
                                  left join ratings r on m.id = r.movieId
                                  left join ratings myr on m.id = myr.movieId 
                                    and myr.userId = @userId
                                  where slug = @slug
                                  group by id, userrating
                                  """, new { slug, userId }, cancellationToken: cancellationToken));
        if (movie is null) return null;

        var genres = await connection.QueryAsync<string>(
            new CommandDefinition("""
                                   
                                  select name from genres where movieId = @id
                                  """, new { id = movie.Id }, cancellationToken: cancellationToken));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(Guid? userId = default, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        var result = await connection.QueryAsync(new CommandDefinition("""
            select m.*, 
                   string_agg(distinct g.name, ',') as genres,
                   round(avg(r.rating), 1) as rating, myr.rating as userrating 
            from Movies m 
            left join Genres g on m.Id = g.movieId
            left join ratings r on m.id = r.movieId
            left join ratings myr on m.id = myr.movieId 
              and myr.userId = @userId
            group by id, userrating
            """, new { userId }, cancellationToken: cancellationToken));

        return result.Select(x => new Movie
        {
            Id = x.id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Rating = (float?)x.rating,
            UserRating = (int?)x.userRating,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });
    }

    public async Task<bool> UpdateAsync(Movie movie, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await connection.ExecuteAsync(
            new CommandDefinition("delete from Genres where movieId = @Id", new { id = movie.Id }));

        foreach (var genre in movie.Genres)
        {
            await connection.ExecuteAsync(new CommandDefinition("""
                insert into genres (movieId, name)
                values (@MovieId, @Name)
                """, new { MovieId = movie.Id, Name = genre }, cancellationToken: cancellationToken));
        }

        var result = await connection.ExecuteAsync(new CommandDefinition("""
               update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease
               where id = @Id
               """, movie, cancellationToken: cancellationToken));
        
        transaction.Commit();
        
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();
        
        await connection.ExecuteAsync(
            new CommandDefinition("delete from Genres where movieId = @id", new { id }));
        
        var result = await connection.ExecuteAsync(new CommandDefinition(
            "delete from Movies where id = @id", new { id }, cancellationToken: cancellationToken));
        
        transaction.Commit();

        return result > 0;

    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync(cancellationToken);
        return await connection.ExecuteScalarAsync<bool>(
            new CommandDefinition(
            "select count(1) from Movies where id = @id", new { id }, cancellationToken: cancellationToken));
                                                                               
    }
}