using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Videogenre
{
    public Guid VideoGenreId { get; set; }

    public Guid VideoId { get; set; }

    public Guid GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
