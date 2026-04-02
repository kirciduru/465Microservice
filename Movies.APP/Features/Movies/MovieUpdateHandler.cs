using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Movies;

public class MovieUpdateRequest : Request, IRequest<CommandResponse>
{
    
}

public class MovieUpdateHandler : Service<Movie>, IRequestHandler<MovieUpdateRequest, CommandResponse>
{
    public MovieUpdateHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(MovieUpdateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}