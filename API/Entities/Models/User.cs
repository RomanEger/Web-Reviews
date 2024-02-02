using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string Nickname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public Guid UserRankId { get; set; }

    public byte[]? Photo { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? RefreshTokenExpires { get; set; }

    public virtual Userrank UserRank { get; set; } = null!;

    public virtual ICollection<Usercomment> Usercomments { get; set; } = new List<Usercomment>();

    public virtual ICollection<Videorating> Videoratings { get; set; } = new List<Videorating>();
}
