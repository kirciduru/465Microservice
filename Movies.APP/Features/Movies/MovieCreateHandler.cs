using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Movies;

public class MovieCreateRequest : Request, IRequest<CommandResponse>
{
    
}

public class MovieCreateHandler : Service<Movie>, IRequestHandler<MovieCreateRequest, CommandResponse>
{
    public MovieCreateHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(MovieCreateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}