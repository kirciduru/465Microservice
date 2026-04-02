using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors;

public class DirectorCreateRequest : Request, IRequest<CommandResponse>
{
    
}

public class DirectorCreateHandler : Service<Director>, IRequestHandler<DirectorCreateRequest, CommandResponse>
{
    public DirectorCreateHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(DirectorCreateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}