using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Userrank
{
    public Guid UserRankId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
