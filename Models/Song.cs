using System;
using System.Collections.Generic;

namespace BDLab2;

public partial class Song
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public int Length { get; set; }

    public int AlbumId { get; set; }

    public int GenreId { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual Genre Genre { get; set; } = null!;
}
