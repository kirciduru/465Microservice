using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP;

public class MoviesDb : DbContext
{
    public DbSet<Movie> Movies { get; set; }
    
    public DbSet<Genre> Genres { get; set; }
    
    public DbSet<Director> Directors { get; set; }
    
    public DbSet<MovieGenre> MovieGenres { get; set; }

    public MoviesDb(DbContextOptions<MoviesDb> options) : base(options)
    {
        
    }
}