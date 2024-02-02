using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Studio
{
    public Guid StudioId { get; set; }

    public string Title { get; set; } = null!;

    public DateTime FoundationDate { get; set; }

    public virtual ICollection<Studiovideo> Studiovideos { get; set; } = new List<Studiovideo>();
}
