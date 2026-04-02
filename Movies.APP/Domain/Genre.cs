using System.ComponentModel.DataAnnotations;
using CORE.APP.Domain;

namespace Movies.APP.Domain;

public class Genre : Entity
{
    [Required, StringLength(100)]
    public string Name { get; set; }

    public List<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
}