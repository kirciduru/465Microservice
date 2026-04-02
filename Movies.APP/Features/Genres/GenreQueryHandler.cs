using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

namespace Movies.APP.Features.Genres;

public class GenreQueryRequest : Request, IRequest<IQueryable<GenreQueryResponse>>
{
    
}

public class GenreQueryResponse : Response
{
    
}

public class GenreQueryHandler : Service<Genre>, IRequestHandler<GenreQueryRequest, IQueryable<GenreQueryResponse>>
{
    public GenreQueryHandler(DbContext db) : base(db)
    {
    }

    public Task<IQueryable<GenreQueryResponse>> Handle(GenreQueryRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}