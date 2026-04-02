using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Directors;

public class DirectorQueryRequest : Request, IRequest<IQueryable<DirectorQueryResponse>>
{
    
}

public class DirectorQueryResponse : Response
{
    
}

public class DirectorQueryHandler : Service<Director>, IRequestHandler<DirectorQueryRequest, IQueryable<DirectorQueryResponse>>
{
    public DirectorQueryHandler(DbContext db) : base(db)
    {
    }

    public Task<IQueryable<DirectorQueryResponse>> Handle(DirectorQueryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}