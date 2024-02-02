using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Videostatus
{
    public Guid VideoStatusId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
