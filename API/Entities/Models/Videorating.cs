using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Videorating
{
    public Guid VideoRatingId { get; set; }

    public int Rating { get; set; }

    public Guid UserId { get; set; }

    public Guid VideoId { get; set; }

    public virtual User User { get; set; } = null!;
}
