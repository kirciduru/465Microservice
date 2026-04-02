using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Genres;

public class GenreUpdaeRequest : Request, IRequest<CommandResponse>
{
    
}

public class GenreUpdateHandler : Service<Genre>, IRequestHandler<GenreUpdaeRequest, CommandResponse>
{
    public GenreUpdateHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(GenreUpdaeRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}