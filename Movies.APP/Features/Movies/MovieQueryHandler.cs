using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using Movies.APP.Features.Directors;
using Movies.APP.Features.Genres;

namespace Movies.APP.Features.Movies;

public class MovieQueryRequest : Request, IRequest<IQueryable<MovieQueryResponse>>
{
    
}

public class MovieQueryResponse : Response
{
    public string Name { get; set; }
    
    public DateTime? ReleaseDate { get; set; }
    
    public double TotalRevenue { get; set; }
    
    public int DirectorId { get; set; }
    
    public List<int> GenreIds { get; set; }
    
    //custom properties
    public string DirectorName { get; set; }
    
    public string ReleaseDateF{ get; set; }
    
    public string TotalRevenueF { get; set; }
    
    public string GenresF { get; set; }

    public List<GenreQueryResponse> Genres { get; set; }
    
    // director query response ekle
    
    public DirectorQueryResponse Director { get; set; } 
    
}

public class MovieQueryHandler : Service<Movie>, IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
{
    public MovieQueryHandler(DbContext db) : base(db)
    {
    }

    protected override IQueryable<Movie> DbSet()
    {
        return base.DbSet().Include(m => m.Director)
            .Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre)
            .OrderByDescending(m => m.ReleaseDate).ThenBy(m => m.TotalRevenue)
            .ThenBy(m => m.Name);
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
            GenreIds = m.MovieGenres.Select(mg => mg.GenreId).ToList(),
            DirectorName = m.Director.FirstName + " " + m.Director.LastName,
            ReleaseDateF = m.ReleaseDate.HasValue ? m.ReleaseDate.Value.ToString("MM/dd/yyyy HH:mm:ss") : string.Empty,
            TotalRevenueF = m.TotalRevenue.ToString("C2"),
            GenresF = string.Join(", ", m.MovieGenres.Select(gg => gg.Genre.Name)),
            Director = new DirectorQueryResponse
            {
                Id = m.Director.Id,
                FirstName = m.Director.FirstName,
                LastName = m.Director.LastName,
                IsRetired = m.Director.IsRetired,
            },
            Genres = m.MovieGenres.Select(gg => new GenreQueryResponse
            {
                Id = gg.Genre.Id,
                Name = gg.Genre.Name
            }).ToList()
        });

        return Task.FromResult(query);
    }
}