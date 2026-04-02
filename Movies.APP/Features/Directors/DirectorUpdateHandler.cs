using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors;

public class DirectorUpdateRequest : Request, IRequest<CommandResponse>
{
    
}
public class DirectorUpdateHandler : Service<Director>, IRequestHandler<DirectorUpdateRequest, CommandResponse>
{
    public DirectorUpdateHandler(DbContext db) : base(db)
    {
    }

    public Task<CommandResponse> Handle(DirectorUpdateRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}