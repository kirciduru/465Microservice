using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;
using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Movies;

public class MovieCreateRequest : Request, IRequest<CommandResponse>
{
    [Required, StringLength(150)]
    public string Name { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public double TotalRevenue { get; set; }

    public int DirectorId { get; set; }

    public List<int> GenreIds { get; set; } = new List<int>();
}

public class MovieCreateHandler : Service<Movie>, IRequestHandler<MovieCreateRequest, CommandResponse>
{
    public MovieCreateHandler(DbContext db) : base(db)
    {
    }

    public async Task<CommandResponse> Handle(MovieCreateRequest request, CancellationToken cancellationToken)
    {
        if (await DbSet().AnyAsync(m => m.Name == request.Name.Trim(), cancellationToken))
            return Error($"Movie with the same name: \"{request.Name.Trim()}\" exists!");

        if (!await DbSet<Director>().AnyAsync(d => d.Id == request.DirectorId, cancellationToken))
            return Error($"Director with ID {request.DirectorId} not found!");

        var genreIds = request.GenreIds?.Distinct().ToList() ?? new List<int>();
        if (genreIds.Any())
        {
            var existingGenreIds = await DbSet<Genre>()
                .Where(g => genreIds.Contains(g.Id))
                .Select(g => g.Id)
                .ToListAsync(cancellationToken);
            var missingGenreIds = genreIds.Except(existingGenreIds).ToList();
            if (missingGenreIds.Any())
                return Error(missingGenreIds.Count == 1
                    ? $"Genre with ID {missingGenreIds[0]} not found!"
                    : $"Genres with IDs {string.Join(", ", missingGenreIds)} not found!");
        }

        var entity = new Movie
        {
            Name = request.Name?.Trim(),
            ReleaseDate = request.ReleaseDate,
            TotalRevenue = request.TotalRevenue,
            DirectorId = request.DirectorId,
            GenreIds = genreIds
        };

        await CreateAsync(entity, cancellationToken);
        return Success($"Movie with name \"{request.Name.Trim()}\" created successfully.", entity.Id);
    }
}
