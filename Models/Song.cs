using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BDLab2;

public partial class Song
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]

    public string Title { get; set; } = null!;
    [Required(ErrorMessage = "Field can't be empty")]

    public int Length { get; set; }

    public int AlbumId { get; set; }

    public int GenreId { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
