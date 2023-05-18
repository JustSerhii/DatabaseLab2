using System;
using System.Collections.Generic;

namespace BDLab2;

public partial class Artist
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int LabelId { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Label Label { get; set; } = null!;
}
