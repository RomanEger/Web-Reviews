using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Genre
{
    public Guid GenreId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Videogenre> Videogenres { get; set; } = new List<Videogenre>();
}
