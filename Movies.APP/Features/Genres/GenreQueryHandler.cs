using CORE.APP.Models;
using CORE.APP.Services;
using Movies.APP.Domain;
using Movies.APP.Features.Movies;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Movies.APP.Features.Genres;

public class GenreQueryRequest : Request, IRequest<IQueryable<GenreQueryResponse>>
{
}

public class GenreQueryResponse : Response
{
    // entity properties Onurcan made :)
    public string Name { get; set; }
    public List<int> MovieIds { get; set; }

    // custom properties Onurcan made :)
    public string MoviesF { get; set; }
    public List<MovieQueryResponse> Movies { get; set; }
}

public class GenreQueryHandler : Service<Genre>, IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
{
    public GenreQueryHandler(DbContext db) : base(db)
    {
    }

    protected override IQueryable<Genre> DbSet()
    {
        return base.DbSet()
            .Include(g => g.MovieGenres).ThenInclude(mg => mg.Movie)
            .OrderBy(g => g.Name);
    }

    public Task<IQueryable<GenreQueryResponse>> Handle(
        GenreQueryRequest request, CancellationToken cancellationToken)
    {
        var query = DbSet().Select(g => new GenreQueryResponse
        {
            Id = g.Id,
            Name = g.Name,
            MovieIds = g.MovieGenres.Select(mg => mg.MovieId).ToList(),
            MoviesF = string.Join(", ", g.MovieGenres.Select(mg => mg.Movie.Name)),
            Movies = g.MovieGenres.Select(mg => new MovieQueryResponse
            {
                Id = mg.Movie.Id,
                Name = mg.Movie.Name
            }).ToList()
        });

        return Task.FromResult(query);
    }
}