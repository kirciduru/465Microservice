using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors;

public class DirectorDeleteRequest : Request, IRequest<CommandResponse>
{
    
}

public class DirectorDeleteHandler : Service<Director>, IRequestHandler<DirectorDeleteRequest, CommandResponse>
{
    public DirectorDeleteHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(DirectorDeleteRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}