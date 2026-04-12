using System.ComponentModel.DataAnnotations;
using CORE.APP.Domain;

namespace Movies.APP.Domain;

public class Director : Entity
{
    [Required, StringLength(150)]
    public string FirstName { get; set; }
    
    [Required, StringLength(150)]
    public string LastName { get; set; }
    
    public bool IsRetired { get; set; }
    
    public List<Movie> Movies { get; set; } = new List<Movie>();
}