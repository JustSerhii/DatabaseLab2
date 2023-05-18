using System;
using System.Collections.Generic;

namespace BDLab2;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
