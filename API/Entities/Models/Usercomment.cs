using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Usercomment
{
    public Guid UserCommentId { get; set; }

    public string Text { get; set; } = null!;

    public Guid UserId { get; set; }

    public Guid VideoId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
