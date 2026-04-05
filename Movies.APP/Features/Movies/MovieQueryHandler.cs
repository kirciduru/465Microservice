using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.APP.Features.Genres;

namespace Movies.APP.Features.Movies;

public class MovieQueryRequest : Request, IRequest<IQueryable<MovieQueryResponse>>
{
    
}

public class MovieQueryResponse : Response
{
    public string Name { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public decimal TotalRevenue { get; set; }
    
    public int DirectorId { get; set; }
    public string DirectorName { get; set; }
    
    public List<int> GenreIds { get; set; }
    public string GenresData { get; set; }
}

public class MovieQueryHandler : Service<Movie>, IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
{
    public MovieQueryHandler(DbContext db) : base(db)
    {
    }

    protected override IQueryable<Movie> DbSet()
    {
        return base.DbSet()
            .Include(m => m.Director)
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .OrderBy(m => m.Name);
    }

    public Task<IQueryable<MovieQueryResponse>> Handle(MovieQueryRequest request, CancellationToken cancellationToken)
    {
        var query = DbSet().Select(m => new MovieQueryResponse
        {
            Id = m.Id,
            Name = m.Name,
            ReleaseDate = m.ReleaseDate,
            TotalRevenue = m.TotalRevenue,
            DirectorId = m.DirectorId,
            DirectorName = m.Director.FirstName + " " + m.Director.LastName,
            GenreIds = m.MovieGenres.Select(mg => mg.GenreId).ToList(),
            GenresData = string.Join(", ", m.MovieGenres.Select(mg => mg.Genre.Name))
        });

        return Task.FromResult(query);
    }
}