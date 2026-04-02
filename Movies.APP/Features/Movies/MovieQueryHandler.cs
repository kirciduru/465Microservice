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
    
}

public class MovieQueryHandler : Service<Movie>, IRequestHandler<MovieQueryRequest, IQueryable<MovieQueryResponse>>
{
    public MovieQueryHandler(DbContext db) : base(db)
    {
    }

    public Task<IQueryable<MovieQueryResponse>> Handle(MovieQueryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}