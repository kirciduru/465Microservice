using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Genres;

public class GenreDeleteRequest : Request, IRequest<CommandResponse>
{
    
}

public class GenreDeleteHandler : Service<Genre>, IRequestHandler<GenreDeleteRequest, CommandResponse>
{
    public GenreDeleteHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(GenreDeleteRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}