using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BDLab2;

public partial class Label
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]

    public string Name { get; set; } = null!;

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
