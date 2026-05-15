using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CORE.APP.Domain;

namespace Movies.APP.Domain;

public class Movie : Entity
{
    [Required, StringLength(150)]
    public string Name { get; set; }
    
    public DateTime? ReleaseDate { get; set; }
    
    public double TotalRevenue { get; set; }
    
    public int DirectorId { get; set; }
    
    public Director Director { get; set; }

    public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    
    [NotMapped]
    public List<int> GenreIds { 
            
        get => MovieGenres.Select(gg => gg.GenreId).ToList(); 
        set => MovieGenres = value.Select(v => new MovieGenre { GenreId = v }).ToList(); 
    }
}
