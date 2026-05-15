using CORE.APP.Models;
using CORE.APP.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movies.APP.Domain;

using System.ComponentModel.DataAnnotations;

namespace Movies.APP.Features.Movies;

public class MovieUpdateRequest : Request, IRequest<CommandResponse>
{
    [Required, StringLength(150)]
    public string Name { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public double TotalRevenue { get; set; }

    public int DirectorId { get; set; }

    public List<int> GenreIds { get; set; } = new List<int>();
}

public class MovieUpdateHandler : Service<Movie>, IRequestHandler<MovieUpdateRequest, CommandResponse>
{
    public MovieUpdateHandler(DbContext db) : base(db)
    {
    }

    public async Task<CommandResponse> Handle(MovieUpdateRequest request, CancellationToken cancellationToken)
    {
        if (await DbSet().AnyAsync(m => m.Id != request.Id && m.Name == request.Name.Trim(), cancellationToken))
            return Error($"Movie with the same name: \"{request.Name.Trim()}\" exists!");

        var entity = await DbSet().Include(m => m.MovieGenres).SingleOrDefaultAsync(m => m.Id == request.Id, cancellationToken);
        if (entity is null)
            return Error("Movie not found!");

        if (request.ReleaseDate.HasValue && request.ReleaseDate.Value.Date > DateTime.Today)
            return Error("Release date cannot be in the future!");

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

        entity.Name = request.Name?.Trim();
        entity.ReleaseDate = request.ReleaseDate;
        entity.TotalRevenue = request.TotalRevenue;
        entity.DirectorId = request.DirectorId;
        entity.GenreIds = genreIds;

        await UpdateAsync(entity, cancellationToken);
        return Success($"Movie with name \"{entity.Name}\" updated successfully.", entity.Id);
    }
}
