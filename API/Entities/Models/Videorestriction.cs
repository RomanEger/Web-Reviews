using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Videorestriction
{
    public Guid VideoRestrictionId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
