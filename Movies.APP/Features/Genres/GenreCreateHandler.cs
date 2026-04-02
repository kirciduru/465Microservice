using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Genres;

public class GenreCreateRequest : Request, IRequest<CommandResponse>
{
    
}

public class GenreCreateHandler : Service<Genre>, IRequestHandler<GenreCreateRequest, CommandResponse>
{
    public GenreCreateHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(GenreCreateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}