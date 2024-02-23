using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Authorvideo
{
    public Guid AuthorVideoId { get; set; }

    public Guid VideoId { get; set; }

    public Guid AuthorId { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
