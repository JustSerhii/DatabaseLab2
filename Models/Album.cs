using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BDLab2;

public partial class Album
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]

    public string Title { get; set; } = null!;
    [Required(ErrorMessage = "Field can't be empty")]

    public decimal Price { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]

    public string Description { get; set; } = null!;

    public int ArtistId { get; set; }

    public virtual Artist Artist { get; set; } = null!;

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
