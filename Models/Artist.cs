using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace BDLab2;

public partial class Artist
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Field can't be empty")]

    public string Name { get; set; } = null!;

    public int LabelId { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual Label Label { get; set; } = null!;
}
