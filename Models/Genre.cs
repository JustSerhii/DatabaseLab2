using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BDLab2;

public partial class Genre
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Field can't be empty")]
    public string Description { get; set; } = null!;

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
