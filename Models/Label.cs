using System;
using System.Collections.Generic;

namespace BDLab2;

public partial class Label
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
