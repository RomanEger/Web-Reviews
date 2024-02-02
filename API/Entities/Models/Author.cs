using System;
using System.Collections.Generic;

namespace Entities.Models;

public partial class Author
{
    public Guid AuthorId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public DateTime Birthday { get; set; }

    public byte[]? Photo { get; set; }

    public Guid AuthorTypeId { get; set; }

    public virtual Authortype AuthorType { get; set; } = null!;

    public virtual ICollection<Authorvideo> Authorvideos { get; set; } = new List<Authorvideo>();
}
