using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Genres;

public class GenreCreateRequest : Request, IRequest<CommandResponse>
{
    [Required, StringLength(100)]
    public string Name { get; set; }
    public List<int> MovieIds { get; set; }
}

public class GenreCreateHandler : Service<Genre>, IRequestHandler<GenreCreateRequest, CommandResponse>
{
    public GenreCreateHandler(DbContext db) : base(db)
    {
    }

    public async Task<CommandResponse> Handle(GenreCreateRequest request, CancellationToken cancellationToken)
    {
        if (await DbSet().AnyAsync(g => g.Name == request.Name.Trim(), cancellationToken))
            return Error($"Genre with the same name: \"{request.Name.Trim()}\" exists!");

        var entity = new Genre
        {
            Name = request.Name?.Trim(),
            MovieGenres = request.MovieIds?.Select(id => new MovieGenre { MovieId = id }).ToList()
                ?? new List<MovieGenre>()
        };

        await CreateAsync(entity, cancellationToken);
        return Success($"Genre with name {request.Name.Trim()} created successfully.", entity.Id);
    }
}