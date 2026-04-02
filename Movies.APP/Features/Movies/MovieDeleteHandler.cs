using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Movies;

public class MovieDeleteRequest : Request, IRequest<CommandResponse>
{
    
}

public class MovieDeleteHandler : Service<Movie>, IRequestHandler<MovieDeleteRequest, CommandResponse>
{
    public MovieDeleteHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(MovieDeleteRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}